import { ConsumerPage } from './app.po';

describe('consumer App', () => {
  let page: ConsumerPage;

  beforeEach(() => {
    page = new ConsumerPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
