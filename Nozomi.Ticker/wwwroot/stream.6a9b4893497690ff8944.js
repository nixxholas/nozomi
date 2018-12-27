(window.webpackJsonp=window.webpackJsonp||[]).push([[8],{5:function(o,a,s){(function(o){
/*!
 * Stream v1.0 (https://htmlstream.com)
 * Copyright Htmlstream
 * Licensed under MIT
 */
!function(o){"use strict";o(function(){o('[data-toggle="tooltip"]').tooltip(),o('[data-toggle="popover"]').popover(),o(".popover-dismiss").popover({trigger:"focus"})}),o(function(){o(".js-navbar-scroll").offset().top>150&&o(".js-navbar-scroll").addClass("navbar-bg-onscroll"),o(window).on("scroll",function(){o(".js-navbar-scroll").offset().top>150?o(".js-navbar-scroll").addClass("navbar-bg-onscroll"):(o(".js-navbar-scroll").removeClass("navbar-bg-onscroll"),o(".js-navbar-scroll").addClass("navbar-bg-onscroll--fade"))})}),o(function(){o("a[href*=#js-scroll-to-]:not([href=#js-scroll-to-])").on("click",function(){if(location.pathname.replace(/^\//,"")===this.pathname.replace(/^\//,"")&&location.hostname===this.hostname){var a=o(this.hash);if((a=a.length?a:o("[name="+this.hash.slice(1)+"]")).length)return o("html,body").animate({scrollTop:a.offset().top-10},1e3),!1}})})}(o)}).call(this,s(0))}},[[5,1,0]]]);