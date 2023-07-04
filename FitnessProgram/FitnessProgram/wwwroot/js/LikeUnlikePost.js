﻿function OnLike() {
    var likeBtn = document.getElementById("like-btn");

    if (likeBtn.checked) {
        Like();
    }
    else {
        UnLike();
    }
}

function Like() {
    $.get(`/likes/likepost/${id}`, () => {
        $.get(`/api/likes/${id}`, (likes) => {
            $('#like-btn').text('Liked');
            likesConnection.invoke("CountChanger", likes);
        })
    });
}

function UnLike() {
    $.get(`/likes/unlikepost/${id}`, () => {
        $.get(`/api/likes/${id}`, (likes) => {
            $('#like-btn').text('Like');
            likesConnection.invoke("CountChanger", likes);
        })
    });
}