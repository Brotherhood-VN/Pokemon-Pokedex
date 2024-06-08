import { DatePipe } from "@angular/common";

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
  }

  interface String {
    toDate(): Date;
    toUTCDate(): Date;
    toStringDate(_format?: string): string;
    toStringTime(_format?: string): string;
    toStringDateTime(_format?: string): string;

    toLowerCaseFirst(): string;
    toUpperCaseFirst(): string;

    generateASCII(_length: number): string;
    generateUpper(_length: number): string;
    random(_length: number): string;
  }

  interface Number {
    toStringLeadingZeros(targetLength: number): string;
    randomBetween(min: number, max: number): number;
  }

  interface Array<T> {
    remove(item: T): Array<T>;
    findDuplicateItems(filterConditions: string | Array<string>): T[];
  }
}

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

Date.prototype.toStringDate = function (_format: string = 'dd/MM/yyyy'): string {
  const _this = this as Date;
  return new DatePipe('en-GB').transform(_this, _format);
}

Date.prototype.toStringTime = function (_format: string = 'HH:mm:ss'): string {
  const _this = this as Date;
  return new DatePipe('en-GB').transform(_this, _format);
}

Date.prototype.toStringDateTime = function (_format: string = 'dd/MM/yyyy HH:mm:ss'): string {
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

String.prototype.toDate = function (): Date {
  const _this = this as string;
  return new Date(_this);
}

String.prototype.toUTCDate = function (): Date {
  const _this = this as string;
  return _this.toDate().toUTCDate();
}

String.prototype.toStringDate = function (_format: string = 'dd/MM/yyyy'): string {
  const _this = this as string;
  return new DatePipe('en-GB').transform(_this, _format);
}

String.prototype.toStringTime = function (_format: string = 'HH:mm:ss'): string {
  const _this = this as string;
  return new DatePipe('en-GB').transform(_this, _format);
}

String.prototype.toStringDateTime = function (_format: string = 'dd/MM/yyyy HH:mm:ss'): string {
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

String.prototype.generateASCII = function (_length: number): string {
  const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
  let result = ' ';
  const charactersLength = characters.length;
  for (let i = 0; i < charactersLength; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
  }

  return result.substring(0, _length + 1);
}

String.prototype.generateUpper = function (_length: number): string {
  const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  let result = ' ';
  const charactersLength = characters.length;
  for (let i = 0; i < charactersLength; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
  }

  return result.substring(0, _length + 1);
}

String.prototype.random = function (_length: number): string {
  return Math.random().toString(36).substring(2, _length);
}

Number.prototype.toStringLeadingZeros = function (targetLength: number): string {
  const _this = this as number;
  return String(_this).padStart(targetLength, '0');
}

Number.prototype.randomBetween = function (min: number, max: number): number {
  return Math.floor(Math.random() * (max - min + 1) + min)
}

Array.prototype.remove = function <T>(this: T[], elem: T): T[] {
  return this.splice(this.indexOf(elem), 1);
}

Array.prototype.findDuplicateItems = function <T>(filterConditions: string | Array<string>): T[] {
  const _this = this as T[];

  let foundElements: T[] = [];
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

  return Array.from(new Set(foundElements));
}


export { };
