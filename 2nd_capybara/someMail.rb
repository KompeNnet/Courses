require 'capybara/dsl'
require 'selenium-webdriver'
require 'open-uri'
require 'securerandom'

class DemoCapybara

  include Capybara::DSL
  Capybara.default_driver = :selenium
  Capybara.register_driver :selenium do |app|
    options = {
      :js_errors => false,
    }
    Capybara::Selenium::Driver.new(app, :browser => :chrome)
  end

  @@elementsID = ["user_username", "user_email", "user_password", "user_password_confirmation", ".data-agreement > span"]
  @@arr = Array.new(2)
  @@session

  def OpenBrowser
    count = ARGV[0].to_i
    if count > 0
      for i in 1..count
        @@session = Capybara::Session.new(:selenium)
        @@session.visit("https://dev.by/registration")
        Register()
        Confirmation()
        @@session.driver.browser.quit
      end      
    end
  end 

  def Register
    GetSRegistrationData()
    FillItIn()
    Agreement()
  end

  def Confirmation
    @@session.visit('https://temp-mail.ru/option/change')
    @@session.fill_in(class: 'form-control', with: @@arr[0])
    @@session.find('#postbut').click
    @@session.find('#click-to-refresh').click
    table = @@session.find('#mails > tbody')
    row = table.first('tr', :text => 'Dev.by')
    link = row.find('a', :class => 'title-subject').click
    @@session.find('body > div.page-content > div > div > div.col-sm-8 > div > div > div.pm-text > div > div > p:nth-child(3) > a').click
    @@session.find('#click-to-delete > span').click
    # p @@arr[0] + "  " + @@arr[1]
    p @@arr
  end

  def GetSRegistrationData()
    @@arr[0] = GetRandString(rand(2..16))
    # @@arr[0] = "qwertyuiop"
    @@arr[1] = GetRandString(rand(6..16))
  end

  def GetRandString(str)
    SecureRandom.hex(str / 2)
  end

  def FillItIn
    @@session.fill_in(@@elementsID[0], with: @@arr[0])
    @@session.fill_in(@@elementsID[1], with: @@arr[0] + "@binka.me")
    @@session.fill_in(@@elementsID[2], with: @@arr[1])
    @@session.fill_in(@@elementsID[3], with: @@arr[1])
    @@session.find(@@elementsID[4]).click
  end

  def Agreement
    @@session.find(".btn").click
    begin
      errors = @@session.find('#new_user > div:nth-child(2) > div > div > div')      
    rescue Exception => e
      return
    end
      Register()
    return
  end
end

demoObj = DemoCapybara.new
demoObj.OpenBrowser