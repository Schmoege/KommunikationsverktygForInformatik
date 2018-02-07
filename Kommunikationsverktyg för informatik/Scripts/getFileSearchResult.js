//alert("fhhgnhb");
$(document).ready(function () {
    $(body).on("click", "#searchBtn", function () {
        alert("Hej");
    });
});
function fuck() {
    var fromDate = $("#from").val();
    var toDate = $("#to").val();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "filesearch/filesearch",
        data: JSON.stringify(post),
        dataType: "json"
    }).done(function (comments) {
        $.each(comments, function (key, item) {
            $('#commentsOn' + id).append(
                '<div id="div' + item.Comment.Id + '" class="commentMain">' +
                '<h5 class="commentAuthor">' + item.User.BlogDisplayName + '</h5>' +
                '<p class="commentContent">' + item.Comment.Content + '</p>' +
                '<p class="commentDate" > Skrevs ' + item.ConvertedDateTime + '</p ></div > ');
        });
    });
}