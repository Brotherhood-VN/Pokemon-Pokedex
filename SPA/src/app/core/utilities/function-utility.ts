import { Injectable } from "@angular/core";
import { KeyValuePair } from "./key-value-utility";

@Injectable({
  providedIn: "root",
})
export class FunctionUtility {
  /**
   * @param str
   * Checks empty - Check 1 string có phải empty hoặc null hoặc undefined ko.
   */
  static checkEmpty(str: any) {
    return !str || /^\s*$/.test(str);
  }

  static toFormData(obj: any, form?: FormData, namespace?: string) {
    let fd = form || new FormData();
    let formKey: string;
    for (var property in obj) {
      if (obj.hasOwnProperty(property)) {
        // namespaced key property
        if (!isNaN(property as any)) {
          // obj is an array
          formKey = namespace ? `${namespace}[${property}]` : property;
        } else {
          // obj is an object
          formKey = namespace ? `${namespace}.${property}` : property;
        }
        if (obj[property] instanceof Date) {
          // the property is a date, so convert it to a string
          fd.append(formKey, obj[property].toISOString());
        } else if (typeof obj[property] === 'object' && !(obj[property] instanceof File)) {
          // the property is an object or an array, but not a File, use recursivity
          this.toFormData(obj[property], fd, formKey);
        } else {
          // the property is a string, number or a File object
          fd.append(formKey, obj[property]);
        }
      }
    }
    return fd;
  }

  static generateGUID(): string {
    return "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx".replace(/[x]/g, (c: any) =>
      (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    ).toUpperCase();
  }

  static generateASCII = function (_length: number) {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let result = ' ';
    const charactersLength = characters.length;
    for (let i = 0; i < charactersLength; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result.substring(0, _length + 1);
  }

  static randomBetween(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1) + min)
  }

  static deleteProperties(obj: any) {
    Object.entries(obj).forEach(([key, val]) => {
      let value = val as any;
      !value || value?.length === 0 ? delete obj[key] : 0;
    });
    return obj;
  }

  static deleteArrayProperties(array: any[], properties: string | string[]) {
    let results = array.map((item: any) => {
      typeof (properties) === 'string' ? delete (item[properties.trim()]) : properties.forEach(key => delete (item[key.trim()]));
      return item;
    });

    return results;
  }

  static getKeyValuePairs(obj: any): KeyValuePair[] {
    let results = Object.entries(obj).map(([key, val]) => {
      return <KeyValuePair>{ key: key, value: val }
    });

    return results;
  }

  static getKeyByValue(obj: any, value: any) {
    return Object.keys(obj).find(key => obj[key] === value);
  }

  static getValueByKey(obj: any, key: any) {
    return Object.values(obj).find(val => obj[key] === val);
  }

  /**
   * Diffs date - compare two days
   * 
   * Note: startDate is less than or equal to endDate
   * @param startDate
   * @param endDate
   * @returns number days
   */
  static diffDate(startDate: string | Date, endDate: string | Date): number {
    // get time of one day (24 hours, 60 minutes, 60 seconds)
    let oneDay = 1000 * 60 * 60 * 24;

    let startTime = startDate.toUTCDate().getTime();
    let endTime = endDate.toUTCDate().getTime();

    return Math.round(Math.abs((startTime - endTime)) / oneDay);
  }
}
