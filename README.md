### Dall-E for Unity Editor

Based on [OpenAI-Unity](https://github.com/RageAgainstThePixel/com.openai.unity "OpenAI-Unity")

A Dall-E for Unity Editor but also included full OpenAI api from the base repo as well.

------------


### Pre-Requiresite
- Open AI / Azure Open AI account.
- Get your API key from the platform that you choose.
- Please follow this [instruction](https://github.com/RageAgainstThePixel/com.openai.unity#unity-scriptable-object "instruction") 

**NOTE** OpenAIConfiguration already included in .gitigore

------------



### How to use
Navigate to menu **Tools/Dall-E** you will see 3 main feature of Dall-E (Generations, Edits, Variations)

###### Generations
Generate 1024x1024 images base on prompt that user input.
**Useful Prompt**

For seamless tile texture for surface.
`Seamless : Can you make seamless tiled patterns for {i}`

`seamless for realistic {i} top view square`

Simple Texture for surface.
`{i} texture for video game`

Texture for particle.
`{i} texture for particle solid black background`

###### Edits
Edit your image base on the mask image and prompt.
input for this feature is image and mask.

Mask indicating which areas should be replaced. The transparent areas of the mask indicate where the image should be edited, and the prompt should describe the full new image.

For more information please [visit](http://https://platform.openai.com/docs/guides/images/edits-dall-e-2-only "visit")

**Useful Prompt**

Fill seam of texture.
`Fill {i} texture`

Change some part of texture.
`{color} {something} with {something}`

###### Variations
Generate 1024x1024 variation of a given image without prompt.


------------
### Disclaimer
**This project was a part of .Net Conf Thailand 2023**
Free use until there was change from the parent [repo](https://github.com/RageAgainstThePixel/com.openai.unity "repo").

------------

