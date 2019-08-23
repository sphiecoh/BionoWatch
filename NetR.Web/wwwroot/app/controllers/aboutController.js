'use strict';
app.controller('aboutController', function ($scope, toaster) {
    toaster.pop('note', "Already Bookmarked", "Already bookmarked for user: ");
   
});