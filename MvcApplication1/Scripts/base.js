function CallAjax(url, data, success, erro) {
    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: success,
        error: erro
    });
}

