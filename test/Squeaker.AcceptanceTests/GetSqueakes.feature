Feature: Get List of Squeakes
    As a user
    I want to see a list of squeakes
    So that I can keep upto date with squeakers

Background:
    Given the following squeakes
        | Cum sociis natoque penatibus et |
        | Praesent augue Sed bibendum.    |
        | Nam a sapien.                   |

Scenario: Can request a list of squeakes
    When I GET /v1/squeakes
    Then the response status should be 200
    And the response body should be valid according to openapi description GetSqueakesListResponse in file ./src/Squeaker.Api/www/squeaker-swagger-spec.json
    And response header X-Total-Count should be 3
