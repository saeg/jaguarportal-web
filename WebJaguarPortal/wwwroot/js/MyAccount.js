
$(document).ready(function () {

    $('#personalForm').submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: urlPersonalForm,
            method: 'POST',
            data: formData,
            success: function (resposne) {

                let html = '<div class="alert ' +
                    ((resposne.status == "success") ? 'alert-success' : 'alert-danger') +
                    ' alert-dismissible fade show float" role="alert">' +
                    resposne.message +
                    '</div>';
                $('#personalForm .summary').html(html);
            },
            error: function (xhr, status, error) {
                console.error('Request error. Status: ' + status + ', Error: ' + error);
            }
        });
    });

    $('#passwordForm').submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: urlPasswordForm,
            method: 'POST',
            data: formData,
            success: function (response) {

                let html = '<div class="alert ' +
                    ((response.status == "success") ? 'alert-success' : 'alert-danger') +
                    ' alert-dismissible fade show float" role="alert">' +
                    response.message +
                    '</div>';
                $('#passwordForm .summary').html(html);
            },
            error: function (xhr, status, error) {
                console.error('Request error. Status: ' + status + ', Error: ' + error);
            }
        });
    });

    $('#apiForm').submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: urlApiForm,
            method: 'POST',
            data: formData,
            success: function (response) {

                let html = '<div class="alert ' +
                    ((response.status == "success") ? 'alert-success' : 'alert-danger') +
                    ' alert-dismissible fade show float" role="alert">' +
                    response.message +
                    '</div>';
                $('#apiForm #ClientSecret').val(response.client_secret);
                $('#apiForm #ClientSecret').parent().find('button').removeAttr('disabled');
                $('#apiForm .summary').html(html);
            },
            error: function (xhr, status, error) {
                console.error('Request error. Status: ' + status + ', Error: ' + error);
            }
        });
    });

    $('.input-copy button').click(function (event) {

        let text = $(this).parent().parent().find('input').val();

        if (text)
            navigator.clipboard.writeText(text)
                .then(() => {
                    console.log('Text copied to clipboard');
                })
                .catch((error) => {
                    console.error('Unable to copy text:', error);
                });
    });


});
