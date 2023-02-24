# Neural Radiance Field (NeRF) using Evergine

In this NeRF sample you can explore Neural Radiance Field technology using Evergine. You can navigate through a NeRF scene and use this code base to extend and explore new possibilities.
We have created a code example that showcases an industrial heat pump that has been generated using a NeRF model. In the example, users can orbit around the model to visualize how the trained model can generate any viewing angle of the heat pump. The images generated are synthetic, meaning that a specific camera angle may never have been captured in the training images, but the AI model is able to predict it.

![NeRF demo](Screenshots/heat_pump.jpg)

## Build and Run

- Required Visual Studio 2022 with.NET6 support
- Required NVidia Graphics card serie 2, 3 or 4.

## Create your own NeRF model

You can generate your own NeRF models and add them to the sample. You will need to record a video or capture a collection of images with your mobile device or video camera of the object or environment you wish to represent. Then, you can train the model using [Instant-ngp](https://github.com/NVlabs/instant-ngp) and generate the transform.json files containing the positions of each image used to train the model, along with the base.ingp file that includes the training performed on the network (we recommend performing more than 35,000 iterations to ensure good model definition).

Our recommendations for capturing images or videos for generating a NeRF model would be:
•	Ensure good lighting. 
•	Use a high-definition HD or 4K camera. 
•	Use a wide-angle lens.
•	Keep a low shutter speed (to avoid blurry images) 
•	Keep the exposure consistent throughout the recording.


---
Powered by [Evergine](https://evergine.com)

LET'S CONNECT!

- [Youtube](https://www.youtube.com/channel/UCpA-X92rxM0OuywdVcir9mA)
- [Twitter](https://twitter.com/EvergineTeam)
- [News](https://evergine.com/news/)