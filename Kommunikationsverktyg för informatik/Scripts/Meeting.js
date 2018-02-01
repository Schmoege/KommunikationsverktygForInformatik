﻿$(document).ready(function ()
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

    $(document).on('click', '#addTime', function ()
    {
        var time = $('#Item1_Time').val();
        if (!time.match(/[0-9]{2}[:][0-9]{2}$/))
        {
            alert("Inga bokstäver i tiden, tack.");
            return false;
        }
        else
        {
            if (parseInt(time.substring(0, 2)) > 24 || (parseInt(time.substring(0, 2)) == 24 && parseInt(time.substring(3, 5)) > 59) || parseInt(time.substring(3, 5)) > 59)
            {
                alert('Det är en 24 timmars klocka som gäller.');
                return false;
            }
            else
            {
                if(parseInt(time.substring(0,2)) == 24)
                {
                    time = "00:" + time.substring(3,5);
                }
                var html = $("#timeList").html();
                html += '<li class="scrollItem">' + time + '</li>';
                $("#timeList").html(html);
            }
        }
    });

    $(document).on('keydown keyup', '#Item1_Date' , function (event)
    {
        $("#times").push("test");
        alert($("#times").val());
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