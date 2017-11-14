Created by Hugo Scott-Slade (@hugosslade) for Glitchers (@glitchers)

Simulate multiple audio listeners by using volume cheaply by sacrificing spatial audio for split-screen. 

1. Add VirtualAudioListener to your listeners
2. Add VirtualAudioSource to your AudioSources

You must still have an AudioListener in your scene.

You can use the built in AudioSource component and the falloff settings there (Logarithmic, Linear, Custom)
with the minDistance and maxDistance as usual.

You can enable or disable the effect easily by setting VirtualAudioManager.simulateMultipleListeners to
either true or false before you setup your AudioSources.
