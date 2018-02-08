﻿$(document).ready(function () {
    $(document).on("click", "#searchBtn", function () {
        var fromDate = $("#from").val();
        var toDate = $("#to").val();

        var dates = {};
        dates.dateFrom = fromDate;
        dates.dateTo = toDate;

        $.ajax({
            url: '/FileSearch/getFileSearchResult',
            type: "POST",
            data: JSON.stringify(dates),
            contentType: 'application/json',
        }).done(function (matchedFiles) {
            $("#fileSearchResult").empty();

            if (matchedFiles.length > 0) {

                var titleAppend = "<h2>Dessa filer postades mellan " + fromDate + " och " + toDate + "<ul>";
                $("#fileSearchResult").append(titleAppend);

                $.each(matchedFiles, function (key, item) {
                    var appendString = "<li><a href=\"/Blog/DownloadFile?downloadFileId=" + item.FileID + "\">" + item.FileName + "</a></li>";
                    $("#fileSearchResult").append(appendString);
                });

                $("#fileSearchResult").append("</ul>");
            } else {
                var noFilesFound = "<h2>Inga filer postades postades mellan " + fromDate + " och " + toDate;
                $("#fileSearchResult").append(noFilesFound);
            }
            
        });
    });
});