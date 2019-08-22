
$.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 16 ||
                    key == 187 ||
                    key == 107 ||
                    //key == 190 || //To stop dot or period
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

$.fn.ForceNumericWithDot =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

$.fn.CnicFormat =
    function () {
        return this.each(function () {
            $(this).keydown(function () {

                //allow  backspace, tab, ctrl+A, escape, carriage return
                if (event.keyCode == 8 || event.keyCode == 9
                                  || event.keyCode == 27 || event.keyCode == 13
                                  || (event.keyCode == 65 && event.ctrlKey === true))
                    return;
                if ((event.keyCode < 48 || event.keyCode > 57 && event.keyCode < 96 || event.keyCode > 105))
                    event.preventDefault();

                var length = $(this).val().length;

                if (length == 5 || length == 13)
                    $(this).val($(this).val() + '-');

                
                   

            });
        });
    };

$.fn.OnlyAlphabets =
    function () {
        return this.each(function () {
            $(this).keydown(function () {
                if ((event.keyCode > 64 && event.keyCode < 91) || (event.keyCode > 96 && event.keyCode < 123)
                    || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 32) {
                    return true;
                }
                else {
                    return false;
                }
            });
        });
    };

function CNICFormat(value)
{
    $('#' + value).keydown(function () {
        
        //allow  backspace, tab, ctrl+A, escape, carriage, arrows(37 to 40), delete(46) return
        if (event.keyCode == 8 || event.keyCode == 9 || (event.keyCode >= 37 && event.keyCode <= 40)
                          || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 46
                          || (event.keyCode == 65 && event.ctrlKey === true))
            return;
        if ((event.keyCode < 48 || event.keyCode > 57))
            event.preventDefault();

        var length = $(this).val().length;

        if (length == 5 || length == 13)
            $(this).val($(this).val() + '-');

    });
}

function MatchFatherNameWithGuardianName() {
    var flag = true;
    var FatherCellNo = $("#FatherCellNo").val();
    var GuardianCellNo = $("#GuardianCellNo").val();

    if (FatherCellNo == GuardianCellNo) {
        alert("Father Cell# and Guardian Cell# should be different.");
        flag = false;
    }

    return flag;
}