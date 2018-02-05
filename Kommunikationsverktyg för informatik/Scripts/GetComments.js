function auto_grow(element) {
    element.style.height = element.scrollHeight + 'px';
}

$(document).ready(function () {


    $('.linkComment').each(function () {
        var id = this.id;
        var events = $._data(document.getElementById(id), "events");
        var hasEvents = (events != null);
        if (!hasEvents) {
            $('#' + id).click(function () {
                if ($('.' + id).children().length > 0) {
                    return;
                }
                else {
                    $('.' + id).append('<div id="cmntText' + id + '">' +
                        '<textarea id="txtarea' + id + '" class="inputComment" rows= "1" onkeyup="auto_grow(this);"></textarea >' +
                        '<input type="submit" id="btnPostComment' + id + '" class="btnComment" value="Kommentera" />' +
                        '</div><div id="commentsOn' + id + '"></div>');

                    var post = {};
                    post.Id = id;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/api/comment/getcomments",
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
                $('#btnPostComment' + id).click(function () {
                    var Information = {};
                    Information.Author = $('#currentUser').val();
                    Information.PostID = id.toString();
                    Information.Content = $('#txtarea' + id).val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/api/comment/postcomment",
                        data: JSON.stringify(Information),
                        dataType: "json"
                    }).done(function (comment) {
                        $('#txtarea' + id).val('');
                        $('#txtarea' + id).height('');
                        var author = $('#displayName').val();
                        $('#commentsOn' + id).prepend(
                            '<div id="div' + comment.Id + '" class="commentMain">' +
                            '<h5 class="commentAuthor">' + author + '</h5>' +
                            '<p class="commentContent">' + comment.Content + '</p>' +
                            '<p class="commentDate" > Skrevs ' + comment.ConvertedDateTime + '</p ></div > ');
                    });
                });
            });
        };
    });
});