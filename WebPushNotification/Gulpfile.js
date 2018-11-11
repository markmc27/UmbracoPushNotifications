var gulp = require('gulp');
var $ = require('gulp-load-plugins')();
var spritesmith = require('gulp.spritesmith');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var concat = require('gulp-concat');
var rename = require('gulp-rename');
var replace = require('gulp-replace');
var stripdebug = require('gulp-strip-debug');
var del = require("del");

var sassPaths = [
];

var jsFiles = [
    'assets/js/modules/*.js',
    'assets/js/app.js'
];

// Datestamp for cache busting
var getStamp = function () {
    var myDate = new Date();

    var myYear = myDate.getFullYear().toString();
    var myMonth = ('0' + (myDate.getMonth() + 1)).slice(-2);
    var myDay = ('0' + myDate.getDate()).slice(-2);
    var mySeconds = myDate.getSeconds().toString();

    var myFullDate = myYear + myMonth + myDay + mySeconds;

    return myFullDate;
};

gulp.task('scripts-prod', ['cachebust'], function () {
    return gulp.src(jsFiles)
        .pipe(concat('app.js'))
        .pipe(gulp.dest('./assets/js/dist'))
        .pipe(rename('app.min.js'))
        .pipe(stripdebug())
        .pipe(uglify())
        .pipe(gulp.dest('./assets/js/dist'));
});

gulp.task('scripts-dev', ['cachebust'], function () {
    return gulp.src(jsFiles)
        .pipe(concat('app.js'))
        .pipe(gulp.dest('./assets/js/dist'))
        .pipe(rename('app.min.js'))
        .pipe(gulp.dest('./assets/js/dist'));
});

gulp.task('build-scripts', [], function () {

});

// Cache busting task
gulp.task('cachebust', function () {
    return gulp.src('./Views/Master.cshtml')
        .pipe(replace(/app.min.js\?([0-9]*)/g, 'app.min.js?' + getStamp()))
        .pipe(replace(/app.js\?([0-9]*)/g, 'app.js?' + getStamp()))
        .pipe(gulp.dest('./Views'));
});

gulp.task('default', [], function () {
    gulp.watch(['assets/js/**/*.js'], ['scripts-dev']);
});