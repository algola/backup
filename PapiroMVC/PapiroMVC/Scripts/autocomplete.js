/// <reference path="jquery-1.6.2-vsdoc.js" />
/// <reference path="jquery-ui.js" />
$(document).ready(function () {
    $('*[data-autocomplete-url]')
        .each(function () {
            $(this).autocomplete({
                source: $(this).data("autocomplete-url")
            });
        });


    $('[data-rel=tooltip]').tooltip({ container: 'body' });
    $('[data-rel=popover]').popover({ container: 'body' });

    var inp = $('.input-validation-error:first').get(0);

    if (inp) {
        inp.focus();
    }

});

$('*[data-autocomplete-url]')
       .each(function () {
           $(this).autocomplete({
               source: $(this).data("autocomplete-url")
           });
       });

$('[data-rel=tooltip]').tooltip({ container: 'body' });
$('[data-rel=popover]').popover({ container: 'body' });


var inp = $('.input-validation-error:first').get(0);

if (inp) {
    inp.focus();
}
