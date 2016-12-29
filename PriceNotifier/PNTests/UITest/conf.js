// conf.js
exports.config = {
    framework: 'jasmine',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    baseUrl: 'http://localhost:59476',
    specs: ['spec.js'],
    jasmineNodeOpts: { defaultTimeoutInterval: 20000 },
    multiCapabilities: [{
        browserName: 'chrome'
    }]
}