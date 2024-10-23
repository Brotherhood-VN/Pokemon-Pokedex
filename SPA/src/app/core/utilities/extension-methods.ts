import { DatePipe } from "@angular/common";

//#region Declare
declare global {
  interface Date {
    toDate(): Date;
    toUTCDate(): Date;

    toStringDate(_format?: string): string;
    toStringTime(_format?: string): string;
    toStringDateTime(_format?: string): string;

    toFirstDateOfMonth(): Date;
    toLastDateOfMonth(): Date;
    toFirstDateOfYear(): Date;
    toLastDateOfYear(): Date;

    toBeginDate(): Date;
    toEndDate(): Date;

    toSeq(): string;

    addDays(days: number): Date;
    addMonths(months: number): Date;
    addYears(years: number): Date;
  }

  interface String {
    toDate(): Date;
    toUTCDate(): Date;
    toStringDate(_format?: string): string;
    toStringTime(_format?: string): string;
    toStringDateTime(_format?: string): string;

    toLowerCaseFirst(): string;
    toUpperCaseFirst(): string;
    toTitleCase(): string;

    isNumeric(): boolean;
    thousandsSeperator(_seperator?: string): string;
  }

  interface Number {
    toStringLeadingZeros(targetLength: number): string;
    thousandsSeperator(_seperator?: string): string;
  }

  interface Array<T> {
    remove(item: T): Array<T>;

    findDuplicateItems(filterConditions: string | Array<string>): Array<T>;

    distinct(): Array<T>;
    distinctBy(key: string): Array<T>;

    sum(): number;
    sumBy(key: string): number;

    deleteProperties(predicate: (value: T) => void): Array<T>;
  }
}
//#endregion

//#region Date
Date.prototype.toDate = function (): Date {
  const _this = this as string;
  return new Date(_this);
}

Date.prototype.toUTCDate = function (): Date {
  const _this = this as Date;
  return new Date(Date.UTC(
    _this.getFullYear(),
    _this.getMonth(),
    _this.getDate(),
    _this.getHours(),
    _this.getMinutes(),
    _this.getSeconds(),
    _this.getMilliseconds()));
}

Date.prototype.toStringDate = function (_format: string = 'MM/dd/yyyy'): string {
  const _this = this as Date;
  return new DatePipe('en-GB').transform(_this, _format);
}

Date.prototype.toStringTime = function (_format: string = 'HH:mm:ss'): string {
  const _this = this as Date;
  return new DatePipe('en-GB').transform(_this, _format);
}

Date.prototype.toStringDateTime = function (_format: string = 'MM/dd/yyyy HH:mm:ss'): string {
  const _this = this as Date;
  return new DatePipe('en-GB').transform(_this, _format);
}

Date.prototype.toFirstDateOfMonth = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), _this.getMonth(), 1);
}

Date.prototype.toLastDateOfMonth = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), _this.getMonth() + 1, 0);
}

Date.prototype.toFirstDateOfYear = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), 0, 1);
}

Date.prototype.toLastDateOfYear = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), 11, 31);
}

Date.prototype.toBeginDate = function (): Date {
  const _this = this as Date;
  _this.setHours(0, 0, 0);
  return _this;
}

Date.prototype.toEndDate = function (): Date {
  const _this = this as Date;
  _this.setHours(23, 59, 59);
  return _this;
}

Date.prototype.toSeq = function (): string {
  const _this = this as Date;
  return new DatePipe('en-GB').transform(_this, 'yyyyMMddHHmmssfff');
}

Date.prototype.addDays = function (days: number): Date {
  const _this = this as Date;
  _this.setDate(_this.getDate() + days);
  return _this;
}

Date.prototype.addMonths = function (months: number): Date {
  const _this = this as Date;
  _this.setMonth(_this.getMonth() + 1 + months);
  return _this;
}

Date.prototype.addYears = function (years: number): Date {
  const _this = this as Date;
  _this.setFullYear(_this.getFullYear() + years);
  return _this;
}
//#endregion

//#region String
String.prototype.toDate = function (): Date {
  const _this = this as string;
  return new Date(_this);
}

String.prototype.toUTCDate = function (): Date {
  const _this = this as string;
  return _this.toDate().toUTCDate();
}

String.prototype.toStringDate = function (_format: string = 'MM/dd/yyyy'): string {
  const _this = this as string;
  return new DatePipe('en-GB').transform(_this, _format);
}

String.prototype.toStringTime = function (_format: string = 'HH:mm:ss'): string {
  const _this = this as string;
  return new DatePipe('en-GB').transform(_this, _format);
}

String.prototype.toStringDateTime = function (_format: string = 'MM/dd/yyyy HH:mm:ss'): string {
  const _this = this as string;
  return new DatePipe('en-GB').transform(_this, _format);
}

String.prototype.toLowerCaseFirst = function (): string {
  const _this = this as string;
  return _this.charAt(0).toLowerCase() + _this.slice(1);
}

String.prototype.toUpperCaseFirst = function (): string {
  const _this = this as string;
  return _this.charAt(0).toUpperCase() + _this.slice(1);
}

String.prototype.toTitleCase = function (): string {
  return this.replace(/\w\S*/g, (txt: string) => txt.charAt(0).toUpperCase() + txt.substring(1).toLowerCase());
};

String.prototype.isNumeric = function (): boolean {
  const _this = this as string;
  return /^-?\d+$/.test(_this);
}

String.prototype.thousandsSeperator = function (_seperator: string = ','): string {
  const _this = this as string;
  if (!_this.isNumeric())
    return '';

  return _this.replace(/\B(?=(\d{3})+(?!\d))/g, _seperator);
}
//#endregion

//#region Number
Number.prototype.toStringLeadingZeros = function (targetLength: number): string {
  const _this = this as number;
  return String(_this).padStart(targetLength, '0');
}

Number.prototype.thousandsSeperator = function (_seperator: string = ','): string {
  return Number(this).toString().replace(/\B(?=(\d{3})+(?!\d))/g, _seperator);
}
//#endregion

//#region Array
Array.prototype.remove = function <T>(this: Array<T>, elem: T): Array<T> {
  return this.splice(this.indexOf(elem), 1);
}

Array.prototype.findDuplicateItems = function <T>(filterConditions: string | Array<string>): Array<T> {
  const _this = this as Array<T>;

  let foundElements: Array<T> = [];
  if (typeof (filterConditions) === 'string') {
    _this.forEach((item: T) => {
      if (_this.filter(x => {
        if (item[filterConditions] instanceof Date)
          return x[filterConditions].toStringDate() == item[filterConditions].toStringDate()
        else
          return x[filterConditions] === item[filterConditions]

      }).length > 1)
        foundElements.push(item);
    });
  }
  else {
    _this.forEach((item: T) => {
      if (_this.filter(x => filterConditions.every(key => {
        if (item[key] instanceof Date)
          return x[key].toStringDate() == item[key].toStringDate()
        else
          return x[key] === item[key]

      })).length > 1)
        foundElements.push(item);
    });
  }

  return foundElements.distinct();
}

Array.prototype.distinct = function <T>(this: Array<T>): Array<T> {
  return Array.from(new Set(this));
}

Array.prototype.distinctBy = function <T>(this: Array<T>, key: string): Array<T> {
  return Array.from(new Set(new Map(this.map(item => [item[key], item])).values()));
}

Array.prototype.sum = function (this: Array<number>): number {
  return this.reduce((accumulator: number, currentValue: number) => accumulator + currentValue, 0);
}

Array.prototype.sumBy = function <T>(this: Array<T>, key: string): number {
  return this.map((item: T) => item[key]).sum();
}

Array.prototype.deleteProperties = function <T>(this: Array<T>, predicate: (value: T) => void): Array<T> {
  let properties = predicate.toString()
    .replace(/[x.=>{};]/g, '').trim()
    .split(/[\s,]+/);

  let results = this.map(item => {
    properties.forEach(key => delete (item[key]))
    return item;
  });

  return results;
}
//#endregion

export { };
