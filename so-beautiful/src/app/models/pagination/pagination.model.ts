export class Pagination {

  // tslint:disable-next-line: variable-name
  private readonly _maxPageSize: number = 50;

  public pageLength!: number; // 總資料數
  // 總資料數

  // tslint:disable-next-line: variable-name
  private _pageSize: number;

  // tslint:disable-next-line: variable-name
  private _pageIndex: number;

  constructor() {
    this._pageIndex = 0;
    this._pageSize = 10;
  }

  // 目前頁碼 - 1
  public get pageIndex(): number {
    return this._pageIndex;
  }

  public set pageIndex(value: number) {
    if (value < 0) {
      this._pageIndex = 0;
    } else {
      this._pageIndex = value;
    }
  }

  // 一頁的項目數
  public get pageSize(): number {
    return this._pageSize;
  }

  public set pageSize(value: number) {
    if (value > this._maxPageSize) {
      this._pageSize = this._maxPageSize;
    } else {
      this._pageSize = value;
    }
  }

}
