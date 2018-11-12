var AccessServerPartialView = function (type, urlAction, data, onSuccess, onError, onFailure) {
  $.ajax({
    type: type,
    url: urlAction,
    dataType: 'html',
    data: data,
    ContentType: 'application/html; charset=utf-8',
    success: onSuccess,
    failure: onFailure,
    error: onError
  });
};