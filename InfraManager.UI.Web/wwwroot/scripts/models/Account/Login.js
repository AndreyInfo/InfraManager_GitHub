﻿define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        ViewModel: function (loginPasswordAuthenticationEnabled) {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            //
            self.loginName = ko.observable('');
            self.password = ko.observable('');
            //
            self.getPasswordEncrypted = function () {
                var retval = '';
                const key = 13;
                var pass = self.password();
                //
                for (var i = 0; i < pass.length; i++)
                    retval += String.fromCharCode(pass[i].charCodeAt(0) ^ key);
                //
                return retval;
            };
            //
            self.login = function (ob) {
                var obj = {
                    'loginName': self.loginName(),
                    'passwordEncrypted': self.getPasswordEncrypted()
                };
                self.ajaxControl.Ajax($('.b-auth-submit'),
                    {
                        dataType: 'json',
                        url: '/accountApi/SignIn',
                        method: 'POST',
                        data: obj
                    },
                    function (response) {
                        if (response.Success == false) {
                            if (response.RedirectUrl) {
                                showSpinner();
                                setLocation(response.RedirectUrl);
                            }
                            else
                                $('.b-auth-error')[0].style.visibility = 'visible';
                        }
                        else if (returnUrl) {
                            showSpinner();
                            window.location.href = returnUrl.replace(/&amp;/g, '&');//TODO alex
                        }
                        else {
                            showSpinner();
                            setLocation('SD/Table');
                        }
                    });
            };
            self.forgotPassword = function (ob) {
                require(['sweetAlert'], function () {
                    swal({
                        title: getTextResource('PasswordRecovery'),
                        text: getTextResource('PasswordRecoveryQuestion'),
                        showCancelButton: true,
                        closeOnConfirm: false,
                        closeOnCancel: true,
                        confirmButtonText: getTextResource('ButtonOK'),
                        cancelButtonText: getTextResource('ButtonCancel')
                    },
                        function (value) {
                            if (value == true) {
                                showSpinner();
                                self.ajaxControl.Ajax(null,
                                    {
                                        dataType: 'json',
                                        url: '/accountApi/RestorePassword',
                                        method: 'POST',
                                        data: { '': self.loginName() }
                                    },
                                    function (response) {
                                        hideSpinner();
                                        swal(getTextResource('PasswordRecovery'), response, 'info');
                                    },
                                    function () {
                                        hideSpinner();
                                        swal(getTextResource('PasswordRecovery'), getTextResource('AjaxError') + '\n[Login.js, forgotPassword]', 'error');
                                    });
                            }
                        });
                });
            };
            self.setLanguage = function (data, event) {
                var target;
                if (event.target) target = event.target;
                else if (event.srcElement) target = event.srcElement;
                //                
                var cultureName = $(target).attr('data-param');
                if (cultureName != locale) {
                    document.cookie = "defaultCulture=" + cultureName;
                    window.location.reload(true);
                }
                //
                /*showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/accountApi/SetUserLanguage',
                        method: 'POST',
                        data: { '': cultureName }
                    },
                    function (response) {
                        if (response == false) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('AjaxError') + '\n[Login.js, setLanguage]', 'error');
                            });
                        }
                        else
                            window.location.reload(true);
                    },
                    null,
                    hideSpinner);*/
            };
            self.onEnter = function (d, e) {
                if (e.keyCode == 13)
                    self.login();
                return true;
            };
            //          
            self.AfterRender = function () {
                getLogoPath('login').done(function (path) {
                    var elem = document.getElementsByClassName('b-auth-logo')[0];
                    elem.style.background = 'url(' + path + ') no-repeat center';
                    elem.style.backgroundSize = 'contain';
                });
                //
                if (loginPasswordAuthenticationEnabled == false)
                    $('#b-auth-forgotPass-link').remove();
                //
                $('.b-auth-showPass').on('click', function () {
                    if ($(this).hasClass('b-auth-showPass__active')) {
                        $(this).removeClass('b-auth-showPass__active');
                        $(this).siblings('input').attr('type', 'password');
                    } else {
                        $(this).addClass('b-auth-showPass__active');
                        $(this).siblings('input').attr('type', 'text');
                    }
                });
                $('#firstInput').focus();
                //      


            };
        }
    }
    return module;
});
