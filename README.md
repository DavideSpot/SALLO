# Suite for the Assessment of Low Level cues on Orientation (SALLO) Unity Package

<p align="center">
  <img src="media/sallo_logo.png">
</p>

Welcome to the Suite for the Assessment of Low Level cues on Orientation (SALLO) Unity Package. SALLO is a comprehensive suite of tools designed to simplify the development of psychophysical experiments focused on spatial orientation in Unity projects.

## Features

SALLO is built on the understanding that psychophysical experiments can be divided into four essential sub-elements:

1. **Stimulus:** Define and manipulate the visual or sensory stimuli presented to participants.
2. **Task:** Implement experimental tasks that participants need to perform in response to the stimuli.
3. **Psychophysical Method:** Apply various psychophysical methods to quantify perceptual experiences.
4. **Spatial Positioning:** Control the spatial arrangement and positioning of elements within the virtual environment.

Therefore, SALLO includes template game objects and base components that cover each sub-element of the psychophysical experiment. The tools included in SALLO provide a reference base that developers can customize easily to their needs, so as to speed up and simplify the experiment development. 

## Installation

Import SALLO in your project using [Unity's Package Manager](https://docs.unity3d.com/Manual/Packages.html).

### Install directly from github
In your open Unity project:
1. Go to `Window` > `Package Manager`.
2. Click on the add (+) button, choosing the `Add package from git URL` option.
3. copypaste the git url to this repository and click import
4. You're all set! You can now start using the SALLO tools in your Unity project.

### install from local folder
currently, there are some issues with importing SALLO from git URL, as the package manifest meta file (package.json.meta) may not be imported. As an alternative, you can import it as a local folder.

To do so, put the content of this repository in your Unity project folder /Packages in _one of the following ways_:
1. Downloading and extract this repo in it
2. Initialize a git folder in it and pull this repo in it.

Then, in your open Unity project:
1. Go to `Window` > `Package Manager`.
2. Click on the add (+) button, choosing the `Add package from disk` option.
3. select the package.json file of this repo
4. You're all set! You can now start using the SALLO tools in your Unity project.

#### Python for Unity
The QUEST implementated in SALLO requires the [Python for Unity](https://docs.unity3d.com/Packages/com.unity.scripting.python@2.1/manual/index.html) package, which in turn requires python 2.7. If you want to use the QUEST algorithm, make sure to have it installed on your machine and added to your path. Check the Python for Unity installation guide for additional info.

## Getting Started

SALLO contains a sample experiment to get you started quickly. The sample experiment requires the [Unity Experiment Framework](https://immersivecognition.com/unity-experiment-framework/). You can install it from their github page, or you can import the unitypackage file of the UXF version provided within the SALLO sample experiment folder.

For more detailed instructions and examples, please refer to the SALLO paper (incoming) and to the [SALLO API Documentation](https://davidespot.github.io/SALLO).

## Contributing

We welcome contributions to the SALLO package! If you have ideas for new features, improvements, or bug fixes, please let us know.
If you develop new tools that you think may be beneficial to the community, please add them to the SALLO package in the corresponding folder.

## Support

If you encounter any issues or have questions about using the SALLO package, feel free to open an issue on the GitHub repository. We'll be happy to assist you!

## License

The Suite for the Assessment of Low Level cues on Orientation (SALLO) package is released under the [GNU General Public License v3.0](LICENSE).

---

Designed and maintained by Davide Esposito (https://www.linkedin.com/in/davide-esposito-438116161/)

*Disclaimer: This software is provided as-is and without any warranty. Use at your own risk.*

---

*Please acknowledge this project if you use it for academic or research purposes by citing:*
>incoming

---
