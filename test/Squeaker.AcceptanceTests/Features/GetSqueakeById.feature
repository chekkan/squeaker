Feature: Get Squeake by its ID
         As a user
         Once I have a squeake's ID
         I want to retrieve detailed information about the squeake
         So that I can gather additional information

Background:
    Given the following squeakes
          | ID       | Text                            |
          | 39700594 | Cum sociis natoque penatibus et |

Scenario: GET squeake by valid ID
          When I GET /api/v1/squeakes/39700594
          Then the response status should be 200
          And response body should be valid according to schema file ./wwwroot/squeake-detail-schema.json
          And response body path $.text should be Cum sociis natoque penatibus et