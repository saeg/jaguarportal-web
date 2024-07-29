
$(document).ready(function () {

    $('#newPasswordForm').submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: urlNewPasswordForm,
            method: 'POST',
            data: formData,
            success: function (resposne) {

                let html = '<div class="alert ' +
                    ((resposne.status == "success") ? 'alert-success' : 'alert-danger') +
                    ' alert-dismissible fade show float" role="alert">' +
                    resposne.message +
                    '</div>';
                $('#newPasswordForm .summary').html(html);
                $('#newPasswordForm')[0].reset();
                if (resposne.status == "success")
                { 
                    $('#newPasswordForm button[type=submit]').fadeOut();
                    $('#newPasswordForm .form').fadeOut();
                }
            },
            error: function (xhr, status, error) {
                console.error('Request error. Status: ' + status + ', Error: ' + error);
            },            
            complete: function () {
                $('#newPasswordForm button[type=submit]').removeAttr("disabled");
                $('#newPasswordForm button[type=submit]').html("Change password");
            },
            beforeSend: function () {
                $('#newPasswordForm button[type=submit]').attr("disabled");
                $('#newPasswordForm button[type=submit]').html("<span class=\"spinner-border spinner-border-sm\" role=\"status\" aria-hidden=\"true\"></span>Changing...");                
            }
        });
    });
});
