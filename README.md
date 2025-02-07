The following are a few screenshots of the Lab2 Functionality
  1. Get All Users and Get One Logging Action Filter and the Request/Response Logging Middleware:
    ![getall+getone](https://github.com/user-attachments/assets/bd9f969f-b203-4562-8802-77d318af9a8b)
  2. Get ID of User That Doesn't Exist:
    ![getidnotfound](https://github.com/user-attachments/assets/09dab0d8-6b86-49b7-bf94-4703963defa8)
  3. Get User with Negative ID:
    ![getnegativeid](https://github.com/user-attachments/assets/e51ebb66-29d1-407c-bf91-a847ad0e1d9d)
  4. Map Student to User:
     ![mapstudentuser](https://github.com/user-attachments/assets/74d4b14c-e3ba-49ad-9606-4e161bb2274b)
  5. Search by Name Logging:
     ![searchbyname](https://github.com/user-attachments/assets/2e9eef03-66d8-4f87-bcf5-e6956d7f2bf2)
  6. Unauthorized Update User:(has correct ID but wrong Email):
      ![updatewithwrongEmail](https://github.com/user-attachments/assets/ced72f60-3a42-47ee-a941-55cc1c5042b6)
  7. Upload IMG Logging:
      ![uploadimg](https://github.com/user-attachments/assets/b61907f2-1503-4f3d-a706-56ca9ac0d639)


  - I created UserApiException to generate custom error messages and a GlobalExceptionHandler to catch and handle all exceptions consistently.
  - LoggingActionFilter starts a stopwatch in OnActionExecuting and stops it in OnActionExecuted to measure execution time and log action details.
  - Request Logging Middleware logs incoming requests before the action filter runs, and logs the response with the status code after.
  - The Reflection happens when mapping Tsource to TDestination which can be seen in the 4th image. 
