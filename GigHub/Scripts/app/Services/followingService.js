﻿var FollowingService = function () {
    var createFollowing = function (followeeId, done, fail) {

        $.post("/api/Followings", { followeeId: followeeId })
            .done(done)
            .fail(fail);
    };

    var deleteFollowing = function (followeeId, done, fail) {
        $.ajax({
            url: "/api/Followings/" + followeeId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    }
}();
