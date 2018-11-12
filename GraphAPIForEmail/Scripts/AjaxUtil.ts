import * as $ from 'https://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.2.3.min';

export class AjaxUtil {
  //field 
  type: string;
  utl: string;
  dataType: string;
  data: any;
  ContentType: string;
  success: any;
  error: any;

  //constructor 
  constructor(type: string) {
    this.type = type
  }

  //function 
  AccessServerPartialView() {
    $.ajax({
      type: this.type,
      url: urlAction,
      dataType: 'html',
      data: data,
      ContentType: 'application/html; charset=utf-8',
      success: onSuccess,
      error: onError
    });
  }
}