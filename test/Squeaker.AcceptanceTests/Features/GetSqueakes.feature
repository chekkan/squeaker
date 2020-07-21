Feature: Get List of Squeakes
    As a user
    I want to see a list of squeakes
    So that I can keep upto date with squeakers

Background:
    Given the following squeakes
        |ID        | Text                            |
        | 39700594 | Cum sociis natoque penatibus et |
        | ea90b094 | Nam a sapien                    |
        | 1028bde8 | Praesent augue Sed bibendum.    |

Scenario: Can request a list of squeakes
    When I GET /api/v1/squeakes
    Then the response status should be 200
    And response body should be valid according to schema file ./wwwroot/list-squeakes-schema.json
    And response header X-Total-Count should be 3

Scenario: Can paginate list of squeakes
   When I GET /api/v1/squeakes?_page=2&_limit=1
   Then the response status should be 200
   And response body path $[0].text should be Nam a sapien
   And response header X-Total-Count should be 3
