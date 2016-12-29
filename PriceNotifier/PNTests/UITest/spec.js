// spec.js
describe('Protractor UI Test', function () {
    var EC = protractor.ExpectedConditions;
    var searchword = element(by.model('productname'));
    var login = element(by.id('email'));
    var password = element(by.id('pass'));
    var goButton = element.all(by.css('a')).first();
    var logInButton = element(by.id('loginbutton'));
    var addToListButton = element.all(by.id('addButton')).first();
    var alreadyInTheListButton = element.all(by.id('inTheListButton')).first();
    var removeButton = element.all(by.id('removeButton')).last();
    var products = element.all(by.repeater('product in dbproducts'));

    function loginIn(a, b) {
        login.sendKeys(a);
        password.sendKeys(b);
        logInButton.click();
    }

    it('should be added to list', function () {
        browser.ignoreSynchronization = true;
        browser.get('/login');
        element.all(by.css('a')).get(3).click();
        loginIn('drow1@mail.ru', '1691811qwe');
        browser.ignoreSynchronization = false;
        searchword.sendKeys('iphone')
            .then(function () {
                addToListButton.click();
                browser.wait(EC.visibilityOf(alreadyInTheListButton), 3000);
                browser.get('/myitems');
                element.all(by.css('li > a')).last().click();
                expect(products.last().getText()).toContain('iPhone');
                removeButton.click();
                browser.sleep(3000);
            });
    });
});