(function () { })();

// comment window
function showDialogSubComment() {
    $("#dialogSubComment").dialog({ width: 350, height: 350, title: "Submit new Comment", resizable: false });
    $("#dialogSubComment").toggle().toggle(300);
};

function reloadComments(postId) {
    $("#btn-hide-comments-" + postId).click();
    $("#btn-load-comments-" + postId).click();
}

// comments diplay
function hideShowReplies(id) {
    $("#comments-" + id).toggle(300);
    $("#btn-hide-replies-" + id).toggle();
    $("#btn-show-replies-" + id).toggle();
};
function load(evt) {
    console.log(evt)
    $(evt).toggle(600);
}

function showDialog() {
    $("#dialogPost").dialog({ width: 350, height: 600, title: "Submit Content", resizable: false });
    $("#dialogPost").toggle().toggle(300);
};

$(function () {
    $('#gallery').poptrox({
        usePopupCaption: true
    });
});