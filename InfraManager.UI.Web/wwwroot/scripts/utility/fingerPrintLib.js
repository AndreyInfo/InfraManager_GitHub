define(['fpJs'], function (Fingerprint2) {
    var module = {
        fHash: null,
        init: function () {
            var self = this;

            //  Инитим отпечаток
            self.fingerprintJs = Fingerprint2;
            if (self.fHash == null) {
                _getFingerprint();
            }

            self.fingerprintJs = null;

            function _getFingerprint() {
                var fpjs = FingerprintJS.load();
                fpjs.then(function (agnt) {
                    agnt.get().then(function (obj) {
                        var hash = FingerprintJS.hashComponents(obj);
                        self.fHash = hash;
                    })
                });
            }

        }
    }
    return module;
});
