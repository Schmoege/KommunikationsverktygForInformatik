$(document).ready(function ()
{
    var validSubject = false;
    var validPlace = false;
    var validDate = false;
    var validTimes = false;
    var validUsers = false;

    $(document).on('click', '#goToCreateMeeting', function ()
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

    $(document).on('click', '#createMeeting', function ()
    {
        isFormValid();
        if (validSubject && validPlace && validDate && validTimes && validUsers)
        {
            var sub = $("#Item1_Subject").val();
            var place = $("#Item1_Place").val();
            var date = $("#Item1_Date").val();
            var creatorMail = $("#creatorMail").val();
            var mails = [];
            var times = [];
            $("#selectedUserList ul .remove input").each(function ()
            {
                mails.push($(this).val());
            });
            $("#timeList .timeSuggestion").each(function ()
            {
                times.push($(this).html());
            });
            $.ajax(
                {
                    url: '/Meeting/CreatedMeetings',
                    type: "POST",
                    data:
                    {
                        subject: sub,
                        place: place,
                        date: date,
                        creatorMail: creatorMail,
                        times: times,
                        mails: mails
                    },
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
            validSubject = false;
            validDate = false;
            validPlace = false;
            validTimes = false;
            validUsers = false;
        }
        else
        {
            alert("Vänligen fyll i formuläret korrekt.");
        }
    });

    $(document).on('click', '#acceptTimesButton', function ()
    {
        var meetingID = parseInt($("#meetingID").val());
        var answer = 0;
        var times = [];
        if ($('input[name=attendance]:checked', '#acceptForm').val() == "can")
        {
            answer = 1;
        }
        $("#can ul .can").each(function ()
        {
            times.push($(this).html());
        });
        if(answer == 1 && times.length == 0)
        {
            alert("Måste välja minst en tid,\nannars välj att du inte kan närvara.");
        }
        else
        {
            $.ajax(
            {
                url: '/Meeting/AnswerMeeting',
                type: "POST",
                data:
                {
                    meetingID: meetingID,
                    answer: answer,
                    times: times
                },
                success: function (data) {
                    $("#Meeting").html(data);
                },
                error: function (xhr, status, error) {
                    var msg = "Response failed with status: " + status + "</br>"
                    + " Error: " + error;
                    $("#Meeting").html(msg);
                }
            });
        }
    });

    $(document).on('click', '.specific', function ()
    {
        var id = parseInt($(this).attr("id"));
        $.ajax({
            url: '/Meeting/SpecificMeeting',
            type: "GET",
            data: { meetingID: id },
            success: function (data) {
                $("#Meeting").html(data);
            },
            error: function (xhr, status, error) {
                var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
                $("#Meeting").html(msg);
            }
        });
    });

    $(document).on('click', '.details', function ()
    {
        var id = parseInt($(this).attr("id"));
        $.ajax({
            url: '/Meeting/MeetingDetails',
            type: "GET",
            data: { meetingID: id },
            success: function (data) {
                $("#Meeting").html(data);
            },
            error: function (xhr, status, error) {
                var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
                $("#Meeting").html(msg);
            }
        });
    });

    $(document).on('click', '#radioCannot', function ()
    {
        $("#can ul li").addClass("disable");
        $("#cannot ul li").addClass("disable");
    });

    $(document).on('click', '#radioCan', function () {
        $("#can ul li").removeClass("disable");
        $("#cannot ul li").removeClass("disable");
    });

    $(document).on('click', '.cannot', function () {
        $(this).toggleClass("cannot can");
        var selected = $(this);
        $("#can ul").append($(selected).clone());
        $(selected).remove();
    });

    $(document).on('click', '.can', function () {
        $(this).toggleClass("cannot can");
        var selected = $(this);
        $("#cannot ul").append($(selected).clone());
        $(selected).remove();
    });

    $(document).on('click', '.add', function ()
    {
        $("#selectedUserList ul #empty").remove();
        $(this).toggleClass("add remove");
        var selected = $(this);
        $("#selectedUserList ul").append($(selected).clone());
        $(selected).remove();
        if($("#userList ul li").length == 0)
        {
            $("#userList ul").html('<li id="empty" class="scrollItem">Empty</li>');
        }
    });

    $(document).on('click', '.remove', function ()
    {
        $("#userList ul #empty").remove();
        $(this).toggleClass("add remove");
        var selected = $(this);
        $("#userList ul").append($(selected).clone());
        $(selected).remove();
        if ($("#selectedUserList ul li").length == 0)
        {
            $("#selectedUserList ul").html('<li id="empty" class="scrollItem">Empty</li>');
        }
    });

    $(document).on('click', '.timeSuggestion', function ()
    {
        $(this).remove();
    });

    function isFormValid()
    {
        if(!$("#Item1_Subject").val() == "" && $("#SubjectVal").html() == "")
        {
            validSubject = true;
        }
        if (!$("#Item1_Place").val() == "" && $("#PlaceVal").html() == "")
        {
            validPlace = true;
        }
        if (!$("#Item1_Date").val() == "" && $("#DateVal").html() == "")
        {
            validDate = true;
        }
        if ($("#timeList li").length != 0)
        {
            validTimes = true;
        }
        if(!$("#selectedUserList ul #empty").length)
        {
            validUsers = true;
        }
    }

    $(document).on('click', '#cancelMeeting', function ()
    {
        $.ajax({
            url: '/Meeting/ViewMeetings',
            type: "GET",
            success: function (data) {
                $("#Meeting").html(data);
            },
            error: function (xhr, status, error) {
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
            alert("Följ formatet för tid (hh:mm), tack.");
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
                html += '<li class="scrollItem timeSuggestion">' + time + '</li>';
                $("#timeList").html(html);
            }
        }
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