READ ME: AR Bread Experience (DDA x ITD ASG1)
Name: Tan Ye Kai Student ID: S10270501 Class: Year 2, Diploma in Immersive Media Unity Version: Unity 6 (6000.0.60f1)



1. Platforms & Hardware Requirements
To run this application successfully, you will need:
Hardware: An Android Device (with Camera support for AR) OR a PC/Mac with a Webcam (if running in Unity Editor).
Software: Android 10.0 or higher (for APK) / Unity 6 (for Project files).
Internet Connection: REQUIRED (The app will not function without internet as it relies on Firebase for Authentication and Database).



2. Installation & Setup
APK Installation: Transfer the .apk file to your Android device and install it. Allow permissions for Camera and Network when prompted.

Unity Editor:
Open the project via Unity Hub.
Ensure Android Build Support is installed.
Ensure the scene Scenes/AccountPage is set as the first scene in Build Settings (Index 0).
IMPORTANT - AR Image Targets: To play the AR portion, you must scan specific images.
The Image Targets are located in the folder: Documentation/ImageTargets (or provided physically).
Please have these images ready on a screen or printed out before playing.


3. Walkthrough & Key Controls

Phase 1: Authentication (Firebase)

Launch: Open the app. You will see the Login/Register screen.
Register: Enter a valid email (e.g., test@gmail.com) and a password (min 6 chars). Click Register.

Note: This creates a Realtime Database entry for the user with 0 Coins.

Login: Use your credentials to log in.
Test Account Credentials (for marking):

Email: tester@gmail.com (Create this account yourself to try it out)
Password: 123456

Phase 2: Main Menu

Home Screen: Displays the user's current Coin balance (synced from DB).
Navigation: Use the bottom/center buttons to navigate to History, Build Game, or Store.
Music Toggle: Use the "Toggle Music" button to mute/unmute the BGM.

Phase 3: Build Bread Game (AR Gameplay)
Objective: Scan image targets to find ingredients and build the perfect Pork Floss Bun.

Controls:
Point the camera at the Ingredient Image Target.
A 3D model of the ingredient will appear.
INTERACTION: A UI button labeled "Bake" will appear on the screen. Tap the UI button (not the 3D model) to collect it.
Winning: Once the bun is baked the user will receive 50 coins.

Phase 4: Bread History (AR Gamepay)
Objective: Scan all 3 images of different bread histories to earn coins.

Controls:
Point the camera at the Ingredient Image Target.
A 3D model of the bread will appear with its history
INTERACTION: A UI button labeled "Close" will appear on the screen. Tap the UI button (not the 3D model) to close the history.
Winning: Scan all 3 images and view its history to recieve 100 coins

Phase 5: The Store

Usage: Spend your earned coins to purchase pastries from the bakery.
Database Interaction: Buying an item updates the coins value in Firebase immediately.
Exit: Click the "back" button to return to the main loop.



5. Limitations & Known Bugs
Lighting Sensitivity: As with all AR Foundation apps, image tracking may fail in low-light environments or if the camera is too close to the screen.

Network Latency: If the internet connection is slow, the "Login" button may take 1-2 seconds to respond. Please wait for the feedback text "Login Successful".


6. Credits & References
   
Code & Plugins:
Unity AR Foundation: Used for Image Tracking.
Firebase SDK for Unity: Used for Authentication and Realtime Database.
TextMeshPro: Used for all UI text rendering.

Assets & 3D Models:
Croissant Model: [https://sketchfab.com/3d-models/baked-goods-d1ae09e3cb8343bc8790b15928452906]
Gourmet Fruit Loaf Model: [https://sketchfab.com/3d-models/baked-goods-d1ae09e3cb8343bc8790b15928452906]
Cranberry Cream Cheese Model: [https://sketchfab.com/3d-models/bread-4a7ffd6578a446e6908016fcaae9784b]

Pork Floss Bun (Semi) Model:  [https://sketchfab.com/3d-models/bread-scan-f5ac20ab3a984cc2a5a9e4ee72567bd1]
Eggs Model:  [https://sketchfab.com/3d-models/plate-with-eggs-232d2df6abfa4e2c88931d8f03baaafa]
Flour Model:  [https://sketchfab.com/3d-models/free-flour-pile-95e25055b79541f69d14d64fd76fd5bd]
Sugar Model:  [https://sketchfab.com/3d-models/sugar-bag-scan-lowpoly-f1d13fe16e2f4b0a858546488b0a6aa5]

Pork Floss Bun image: [https://eatbook.sg/best-floss-buns-singapore/]
Croissant imgae: [https://www.google.com/search?q=croissant+breadtalk&sca_esv=be6047ef5dda0c71&rlz=1C1CHBF_en-GBSG1105SG1105&udm=2&biw=1440&bih=731&sxsrf=AE3TifNxrkuIefohVsOOet4h4GUYH-UuKg%3A1765717611635&ei=a7Y-aZqtJrWz4-EPvfTJoQU&ved=0ahUKEwjajo_Ykr2RAxW12TgGHT16MlQQ4dUDCBI&uact=5&oq=croissant+breadtalk&gs_lp=Egtnd3Mtd2l6LWltZyITY3JvaXNzYW50IGJyZWFkdGFsazIHECMYyQIYJzIGEAAYBxgeMgYQABgHGB4yBRAAGIAEMgYQABgFGB4yBhAAGAUYHjIGEAAYBRgeMgYQABgFGB4yBhAAGAUYHjIGEAAYBRgeSPQcUNkQWKUbcAN4AJABAJgB3QOgAeYIqgEJNy4wLjEuMC4xuAEDyAEA-AEBmAIKoAKsA8ICBBAAGB7CAgYQABgIGB7CAggQABgIGAcYHpgDAIgGAZIHAjEwoAeCOLIHATe4B58DwgcHMC4zLjYuMcgHKYAIAQ&sclient=gws-wiz-img#sv=CAMSVhoyKhBlLU03X2FwUjZTZEQ2Q3RNMg5NN19hcFI2U2RENkN0TToORlJ5NC1oN09SSjMydE0gBCocCgZtb3NhaWMSEGUtTTdfYXBSNlNkRDZDdE0YADABGAcggrOShgkwAkoKCAIQAhgCIAIoAg]
Cranberry Cream Cheese image: [https://www.google.com/search?q=cranberry+cream+cheese+bread+breadtalk&sca_esv=be6047ef5dda0c71&rlz=1C1CHBF_en-GBSG1105SG1105&udm=2&biw=1440&bih=731&sxsrf=AE3TifM0897-L6C9BK4zD5ORMh3KmDmFcw%3A1765717786832&ei=Grc-aZS8MqjG4-EPxcuW4Ak&oq=cranberry+breadtalk&gs_lp=Egtnd3Mtd2l6LWltZyITY3JhbmJlcnJ5IGJyZWFkdGFsayoCCAAyBhAAGAcYHjIGEAAYBxgeMgYQABgHGB4yBhAAGAcYHjIGEAAYBxgeMgYQABgHGB4yBhAAGAcYHjIFEAAYgAQyCBAAGAUYBxgeMgYQABgIGB5IruYEUIq9BFjn2ARwBXgAkAEAmAFOoAGfBaoBAjEzuAEByAEA-AEBmAIOoAL3BMICBxAjGMkCGCfCAggQABgIGAcYHpgDAIgGAZIHAjE0oAf5N7IHAjEwuAfoBMIHBzAuNi42LjLIB0aACAE&sclient=gws-wiz-img#sv=CAMSVhoyKhBlLWt0UzRUT0FCU0p5REJNMg5rdFM0VE9BQlNKeURCTToOODRfdXNsNkJFbzFRTU0gBCocCgZtb3NhaWMSEGUta3RTNFRPQUJTSnlEQk0YADABGAcglOa0jgswAkoKCAIQAhgCIAIoAg]
Gourmet Fruit Loaf image: [https://www.google.com/search?q=gourmet+fruit+loaf+breadtalk&sca_esv=be6047ef5dda0c71&rlz=1C1CHBF_en-GBSG1105SG1105&udm=2&biw=1440&bih=731&sxsrf=AE3TifN0yeMbjiGj98oRrNDjZUnZG1s9Ew%3A1765717874447&ei=crc-aeD_Guqr4-EPosnZ2QE&oq=gour+breadtalk&gs_lp=Egtnd3Mtd2l6LWltZyIOZ291ciBicmVhZHRhbGsqAggAMgYQABgHGB4yCBAAGAgYBxgeSOwOUABY7wRwAHgAkAEAmAE-oAHIAaoBATS4AQHIAQD4AQGYAgSgAuEBmAMAkgcBNKAH_w6yBwE0uAfhAcIHBzAuMi4xLjHIBxCACAE&sclient=gws-wiz-img#sv=CAMSVhoyKhBlLUZ5SVFSY3RHNGpzdkdNMg5GeUlRUmN0RzRqc3ZHTToOR0lWSDZMeVlwVjFIcU0gBCocCgZtb3NhaWMSEGUtRnlJUVJjdEc0anN2R00YADABGAcgod_ebDACSgoIAhACGAIgAigC]
Chocolate Croissant image: [https://www.google.com/search?q=chocolate+croissant+breadtalk&sca_esv=be6047ef5dda0c71&rlz=1C1CHBF_en-GBSG1105SG1105&udm=2&biw=1440&bih=731&sxsrf=AE3TifMQx8WMuWZt5QymiFdf71695fjkfw%3A1765717923787&ei=o7c-acXmL-ne4-EPjumPgQ8&oq=chocolate+breadtalk&gs_lp=Egtnd3Mtd2l6LWltZyITY2hvY29sYXRlIGJyZWFkdGFsayoCCAEyBhAAGAcYHjIGEAAYBxgeMgYQABgHGB4yBhAAGAcYHjIIEAAYBxgeGAoyBhAAGAcYHjIGEAAYBxgeMgYQABgHGB4yBhAAGAcYHjIGEAAYBxgeSLcdUABYhRFwAHgAkAEAmAFJoAGQBKoBAjEwuAEByAEA-AEBmAIKoALbBJgDAJIHAjEwoAf3N7IHAjEwuAfbBMIHBzAuMi41LjPIBzqACAE&sclient=gws-wiz-img#sv=CAMSVhoyKhBlLU03X2FwUjZTZEQ2Q3RNMg5NN19hcFI2U2RENkN0TToORlJ5NC1oN09SSjMydE0gBCocCgZtb3NhaWMSEGUtTTdfYXBSNlNkRDZDdE0YADABGAcgnp2KowQwAkoKCAIQAhgCIAIoAg]
Ham and Cheese image: [https://www.google.com/search?q=ham+and+cheese+breadtalk&sca_esv=be6047ef5dda0c71&rlz=1C1CHBF_en-GBSG1105SG1105&udm=2&biw=1440&bih=731&sxsrf=AE3TifOYtHTOuEscjpMHkZhsYUYGlhGFyA%3A1765718013586&ei=_bc-aam6I4Sc4-EP6uKEwAI&ved=0ahUKEwjprOSXlL2RAxUEzjgGHWoxASgQ4dUDCBI&uact=5&oq=ham+and+cheese+breadtalk&gs_lp=Egtnd3Mtd2l6LWltZyIYaGFtIGFuZCBjaGVlc2UgYnJlYWR0YWxrMgcQIxjJAhgnMgYQABgHGB4yCBAAGAcYHhgKMgYQABgHGB4yBRAAGIAEMgYQABgFGB4yBhAAGAgYHjIGEAAYCBgeMgYQABgIGB4yBhAAGAgYHki2DFAAWJ0LcAB4AJABAJgBb6ABigaqAQQxMS4xuAEDyAEA-AEBmAIKoAKsBcICCBAAGAgYBxgewgIIEAAYBRgHGB6YAwCSBwM4LjKgB6NMsgcDOC4yuAesBcIHCTAuMy40LjIuMcgHToAIAQ&sclient=gws-wiz-img#sv=CAMSVhoyKhBlLU9CdWxKOUt5SWNOdlRNMg5PQnVsSjlLeUljTnZUTToObUdmV2xHT3ZxaFNHU00gBCocCgZtb3NhaWMSEGUtT0J1bEo5S3lJY052VE0YADABGAcgi8rUzQMwAkoKCAIQAhgCIAIoAg]
Sugar Donut image: [https://www.google.com/search?q=sugar+donut+breadtalk&sca_esv=be6047ef5dda0c71&rlz=1C1CHBF_en-GBSG1105SG1105&udm=2&biw=1440&bih=731&sxsrf=AE3TifOJbLIi-rZQ-l7w0AYf867yQ8fFBg%3A1765718038919&ei=Frg-af3qN7WGg8UPwNeCqAY&ved=0ahUKEwj9ze6jlL2RAxU1w6ACHcCrAGUQ4dUDCBI&uact=5&oq=sugar+donut+breadtalk&gs_lp=Egtnd3Mtd2l6LWltZyIVc3VnYXIgZG9udXQgYnJlYWR0YWxrMgcQIxjJAhgnMgYQABgIGB4yBhAAGAgYHkjvG1AAWPAZcAF4AJABAJgBU6AByQWqAQIxM7gBA8gBAPgBAZgCDaAC8AXCAgYQABgHGB7CAggQABgIGAcYHsICBhAAGAUYHpgDAJIHAjEzoAeKJ7IHAjEyuAfcBcIHBzAuNi40LjPIB0uACAE&sclient=gws-wiz-img#sv=CAMSVhoyKhBlLXBWcXh3dzZ4WGdwRklNMg5wVnF4d3c2eFhncEZJTToOV0plYmZiWGNvZ0xWTk0gBCocCgZtb3NhaWMSEGUtcFZxeHd3NnhYZ3BGSU0YADABGAcgn_XD1AYwAkoKCAIQAhgCIAIoAg]

Background Music: [[Source Name, e.g., Pixabay "French Cafe Jazz" by ArtistName](https://pixabay.com/music/traditional-jazz-podcast-soothing-serene-smooth-jazz-romantic-cafe-vibes-213058/)]

