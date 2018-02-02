$(document).ready(function ()
{
    $("#goToCreateMeeting").on('click', function ()
    {
        $.ajax({
            url: '/Meeting/CreateMeeting',
            type: "GET",
            success: function (data)
            {
                $("#Meeting").html(data);
            },
            error: function (xhr, status, error)
            {
                var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
                $("#Meeting").html(msg);
            }
        });
    });
    
    $(document).on('keydown keyup', '#Item1_Date' , function (event)
    {
        var inputLength = event.target.value.length;
        if (event.keyCode != 8)
        {
            if (inputLength === 4 || inputLength === 7)
            {
                var thisVal = event.target.value;
                thisVal += '-';
                $(event.target).val(thisVal);
            }
        }
    });

    $(document).on('keydown keyup', '#Item1_Time', function (event)
    {
        var inputLength = event.target.value.length;
        if (event.keyCode != 8)
        {
            if (inputLength === 2)
            {
                var thisVal = event.target.value;
                thisVal += ':';
                $(event.target).val(thisVal);
            }
        }
    });

    document.addEventListener("touchstart", function () { }, true)
});