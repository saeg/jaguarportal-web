
$(document).ready(function () {

    $('#forgotPasswordForm').submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: urlForgotPasswordForm,
            method: 'POST',
            data: formData,
            success: function (resposne) {

                let html = '<div class="alert ' +
                    ((resposne.status == "success") ? 'alert-success' : 'alert-danger') +
                    ' alert-dismissible fade show float" role="alert">' +
                    resposne.message +
                    '</div>';
                $('#forgotPasswordForm .summary').html(html);
            },
            error: function (xhr, status, error) {
                console.error('Request error. Status: ' + status + ', Error: ' + error);
            },            
            complete: function () {
                $('#forgotPasswordForm button[type=submit]').removeAttr("disabled");
                $('#forgotPasswordForm button[type=submit]').html("Request password");                
            },
            beforeSend: function () {
                $('#forgotPasswordForm button[type=submit]').attr("disabled");
                $('#forgotPasswordForm button[type=submit]').html("<span class=\"spinner-border spinner-border-sm\" role=\"status\" aria-hidden=\"true\"></span>Sending...");                
            }
        });
    });
});
