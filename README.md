# GitHubReleaseUpdater
A simple tool used to update and manage GitHub releases.

The initial purpose for this application is to let me keep track some interesting GitHub projects, mainly for some homebrew apps and plugins. Later, I found it's really convinient to update and manage these plguins. 
Some of my friends asked me to share this simple applicaiton, thus I decided to put it here to share with anyone needed.
![image](https://github.com/user-attachments/assets/e846fb66-1fc2-46b6-a9e5-7cb87f5aacfc)


# How to Use

* First of all, GitHub requires user credential to call its API, simple user cred is limitted (~20 access/hour). 
Therefore, I have to using GitHub JWT user token, so users can fetch hundreds of repositories every single time.
* You can simply get your GitHub user token at https://github.com/settings/tokens , and then [Personal access token] > [Tokens(classic)] > "Create an personal access token for C# App". 
You just need a check [repo] and [user] scopes, that's it!
<img src="https://github.com/user-attachments/assets/2969365a-c20d-4eb7-9f43-208f4b431023" width=80% height=80%>

* Then you can input your username and user token in the app.
<img src="https://github.com/user-attachments/assets/105e4e54-b664-4306-9b37-b61fe76c17c5" width=60% height=60%>

* You can then manually add a new user-repository to trach with by input:
  - [User]: the user name who owns the targeting repositories
  - [Repository]: the repo you need to track with, only User and Repository are the real entities needed to fetch data through GitHub
  - [Category]: this only used for categorization, you can put whatever you want
  - [LocalPath]: the local path you wanna keep track of the branch. Since you can download the whole repo checkin, it would be better to keep it in a separate folder, otherwise it would look really messy.

    !! You don't need to fill in the table below !!
  - [Current Release]: the most recent release you have downloaded into your local directory, it would update everytime you download a new release
  - [Latest Release]: this will only update once you try to load data from GitHub
  - [Last Update Time]: the last time you did the download/update

After the user-repo-path information provided, you can simply click on [Load] button to do a simple fetch.
Several seconds may need to do a GitHub load, and all related information will show in the app.
It will also list all the past, present, and preview releases. You can download whichever release you want, and download all are supported as well, but time taken for large repo.
<img src="https://github.com/user-attachments/assets/a6ea67c8-5c2c-4009-8db2-fd31f5fa4915" width=60% height=60%>

# Format
It will save all the repos list in a directory under repo/ folder, you can maintain your list here. 

# TODO
It's a very easy-to-use application to simplfy my labor, I didn't verify and test too many edge case. Thus, many bugs should exist and I still have many things need to do to complete this app.
for example, add a button or view to let user create a new list. (for me, currently I just need to create a .txt file under /repo :p)
Moreover, alerts and popup error need to be handled as well. (I'm lazy to put warning for my own.)

# Language
This app is written in ASP.Net MVC, using GitHub Octokit library for connecting and loading.

