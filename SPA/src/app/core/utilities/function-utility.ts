import { Injectable } from "@angular/core";

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

  /**
  * Append property HttpParams
  * @param str
  * Viết Hoa chữ cái đầu tiên trong chuỗi
  */
  static toUpperCaseFirst(str: string) {
    return str.charAt(0).toUpperCase() + str.slice(1);
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
    );
  }

  static generateASCII = function (_length: number) {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = ' ';
    const charactersLength = characters.length;
    for (let i = 0; i < charactersLength; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result.substring(0, _length + 1).toUpperCase();
  }
}
