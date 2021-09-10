# ChooseYourOwnAdventure

## My Example Decision Heirarchy

Click here to learn about social networks

1. Twitter
    1. What is twitter?
        1. Twitter is an American microblogging and social networking service on which users post and interact with messages known as "tweets". Click to know more.
            1. Go to google.com and search twitter :)
    2. How to post on twitter?
        1. Go to twitter.com, sign up/sign in and type in some message and click the Send icon. Click here what more you can do on twitter.
            1. Follow other profiles
            2. Chat with the people who follow you
2. Facebook
    1. What is facebook?
        1. Facebook, Inc., is an American multinational technology company based in Menlo Park, California.
    2. How to post on facebook?
        1. Go to facebook.com, sign up/sign in and then on your timeline type in some message and click the Post button

## Approach

Let’s discuss the tech stack and a possible architecture to solve this problem.

### Technology Stack

#### Backend

**Database:** SQL Server

**API:** .NET Core Web API

#### Frontend

Vue.js, HTML, CSS, JavaScript


### Back End

#### Database

**Name:** Decisions

**Schema:** Here we are using one Primary Key `DecisionId`, a `Description` of the decision and a Foreign Key to reference the same table with column `ParentDecisionId` so that we can store child decisions.

Here is the create script.

```
CREATE TABLE Decisions
(
	DecisionId INT PRIMARY KEY IDENTITY(1, 1),
	Description VARCHAR(200) NOT NULL,
	ParentDecisionId INT REFERENCES Decisions(DecisionId)
)
```
**Stored Procuedure:**

A stored procedure is needed in order to get all decisions with their details ordered by a depth column set as 0 for the root question and then incremented as we load the children.

```
USE [ChooseYourOwnAdventure]
GO

CREATE Procedure GetDecisions
AS

	With DecisionCTE (DecisionId, ParentDecisionId, Description, Depth)
	As
	(
		Select
			DecisionId,
			ParentDecisionId,
			Description,
			0 As Depth
		From 
			dbo.Decisions
		Where 
			DecisionId = 1

		Union All

		Select
			Child.DecisionId,
			Child.ParentDecisionId,
			Child.Description,
			Parent.Depth + 1 As Depth
		From 
			dbo.Decisions As Child
		Join 
			DecisionCTE As Parent
		On 
			Child.ParentDecisionId = Parent.DecisionId
	)

	Select DecisionId, ParentDecisionId, Description, Depth
	From DecisionCTE
	Order By Depth, DecisionId
GO
```
##### Table
Decisions
| Column | Details |
| --- | ----------- |
| DecisionId | INT PRIMARY KEY IDENTITY(1, 1) |
| Description | VARCHAR (200), NOT NULL |
| ParentDecisionId | INT NULL REFERENCES Decisions (DecisionId) |

#### API
##### Endpoint
```
GET api/DecisionHelper
```
##### Response Example
```
{
	"decisionId": 1,
	"description": "Click here to learn about social networks",
	"parentDecisionId": null,
	"depth": 0,
	"parentDecision": null,
	"childDecisions": [{
		"decisionId": 2,
		"description": "Twitter",
		"parentDecisionId": 1,
		"depth": 1,
		"childDecisions": [{
			"decisionId": 4,
			"description": "What is twitter?",
			"parentDecisionId": 2,
			"depth": 2,
			"childDecisions": [{
				"decisionId": 6,
				"description": "Twitter is an American microblogging and social networking service on which users post and interact with messages known as \"tweets\". Click to know more.",
				"parentDecisionId": 4,
				"depth": 3,
				"childDecisions": [{
					"decisionId": 12,
					"description": "Go to google.com and search twitter :)",
					"parentDecisionId": 6,
					"depth": 4,
					"childDecisions": []
				}]
			}]
		}, {
			"decisionId": 5,
			"description": "How to post on twitter?",
			"parentDecisionId": 2,
			"depth": 2,
			"childDecisions": [{
				"decisionId": 7,
				"description": "Go to twitter.com, sign up/sign in and type in some message and click the Send icon. Click here what more you can do on twitter.",
				"parentDecisionId": 5,
				"depth": 3,
				"childDecisions": [{
					"decisionId": 13,
					"description": "Follow other profiles",
					"parentDecisionId": 7,
					"depth": 4,
					"childDecisions": []
				}, {
					"decisionId": 14,
					"description": "Chat with the people who follow you",
					"parentDecisionId": 7,
					"depth": 4,
					"childDecisions": []
				}]
			}]
		}]
	}, {
		"decisionId": 3,
		"description": "Facebook",
		"parentDecisionId": 1,
		"depth": 1,
		"childDecisions": [{
			"decisionId": 8,
			"description": "What is facebook?",
			"parentDecisionId": 3,
			"depth": 2,
			"childDecisions": [{
				"decisionId": 10,
				"description": "Facebook, Inc., is an American multinational technology company based in Menlo Park, California.",
				"parentDecisionId": 8,
				"depth": 3,
				"childDecisions": []
			}]
		}, {
			"decisionId": 9,
			"description": "How to post on facebook?",
			"parentDecisionId": 3,
			"depth": 2,
			"childDecisions": [{
				"decisionId": 11,
				"description": "Go to facebook.com, sign up/sign in and then on your timeline type in some message and click the Post button.",
				"parentDecisionId": 9,
				"depth": 3,
				"childDecisions": []
			}]
		}]
	}]
}
```
#### Front End

The step-by-step logic can be interpreted as below.

1. Call the API endpointapi `/DecisionHelper` and get theof decision with its child decisions.
2. Display the first decision only
3. Render the decision with the description
    1. If there are child decisions, then render the child decisions
        1. With on click of any of these children, display the child decision
        2. For tracking, emit an event on the bus and send the selected decision id
    2. If no answers are available, then it is a terminal decision
        1. Emit an event on the bus to indicate to render the whole tree
        2. Render the whole decision tree structure
        3. Indicate the decisions that are tracked on the tree
