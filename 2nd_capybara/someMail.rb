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

  def OpenBrowser
    count = ARGV[0].to_i
    if count > 0
      for i in 1..count
        visit("https://dev.by/registration")
        Register()
        Confirmation()
      end      
    end
  end 

  def Register
    GetStrs()
    FillItIn()
    Agreement()
  end

  def Confirmation
    visit('https://temp-mail.ru/option/change')
    fill_in(class: 'form-control', with: @@arr[0])
    find('#postbut').click
    find('#click-to-refresh').click
    table = find('#mails > tbody')
    row = table.first('tr', :text => 'Dev.by')
    link = row.find('a', :class => 'title-subject').click
    find('body > div.page-content > div > div > div.col-sm-8 > div > div > div.pm-text > div > div > p:nth-child(3) > a').click
    p @@arr
  end

  def GetStrs()
    @@arr[0] = GetRandString(rand(2..16))
    @@arr[1] = GetRandString(rand(6..16))
  end

  def GetRandString(str)
    SecureRandom.hex(str / 2)
  end

  def FillItIn
    fill_in(@@elementsID[0], with: @@arr[0])
    fill_in(@@elementsID[1], with: @@arr[0] + "@binka.me")
    fill_in(@@elementsID[2], with: @@arr[1])
    fill_in(@@elementsID[3], with: @@arr[1])
    find(@@elementsID[4]).click
  end

  def Agreement
    find(".btn").click
    begin
      errors = find('//*[@id="new_user"]/div[2]/div/div/div')      
    rescue Exception => e
      return
    end
    Register()
  end
end

demoObj = DemoCapybara.new
demoObj.OpenBrowser