# Overview

I've always been interested in video games. However, my interest in playing has bled over into an interest in making. However, making video games is a huge undertaking, involving much more than code. You need to consider sound design, graphic design, music, quality of play, level design, and even some IT stuff for online games. Although I love coding, there is a reason that I am not an art major (i.e. I have little to no artistic talent), and I have always shyed away from making games.  
Because of this, all of the graphical assets in this game are taken from real photos taken by me and my sister. The result is somewhat strange (especially for me, the subject of the character photos), but it works for what I am trying to do.

I built the skeleton of a simple combat-oriented 2d platformer. I would have built more, but Unity crashed and erased a bunch of my progress. The game (when and if finished) follows Bryan, a young college student, as he navigates a enemy-filled campus on his way to calculus class.  
Because this is a Unity project that I have not published, it isn't really possible to play the game. However, here are the controls:
| Key | Effect |
|-----|--------|
| A   | move left. |
| D   | move right. | 
| S   | crouch. |
| W   | dash forward. |
| space bar | jump (release to stop rising). |
| left mouse button | attack. |

{Describe your purpose for writing this software.}

Aside from the fun of giving a picture of myself the power to send cars flying through the air, I wrote this because I wanted to learn more about the Unity Game Engine. Unity is a very powerful tool, but it is no good if you don't know how to use it. The only effective way to learn how to use it is to practice, so I took this oppurtunity to boost my Unity skills.

Here's a video of me playing the game (and discussing the code):  
[Software Demo Video](https://youtu.be/X84fUhD8fII)

# Development Environment

I built this game using VScode and the free version of Unity Engine.

Unity scripts are written in C#, an object-oriented programming language with C-like syntax. According to the [official microsoft page](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/), C# is also "component-oriented". I'm not really sure what this means, but apparently it makes C# really good for working with systems like Unity where you have existing objects and tack scripts onto them. C# was OK, although I didn't enjoy it as much as I might have because VScode refused to give me intellisense or knowledge of the inner workings of the **UnityEngine** library, which was a critical part of the project. This meant that I had to write some random code and then switch to the Unity editor to see if it worked or not. I didn't get any other error indication, which made writing in this language very irritating. Additionally, the C# standard for functions and libraries is to Camel Case with the first letter capitalized as well. This was tough for me to get used to, since I'm accustomed to the Python standard of snake case. The only other library I used was the **System** library for the `Math.Abs()` function.

# Useful Websites

* [Unity Forums](https://forum.unity.com/)
* [*Brackeys* Youtube channel](https://www.youtube.com/@Brackeys)
* [StackOverflow (of course)](https://stackoverflow.com/)

# Future Work

* Create some actual levels (the game functionality is pretty solid, but I need to design some levels.)
* Make HP actually work
* Compose and add some music
* Add more enemy variety (different kinds of cars)
* Add more moves that you learn as you progress throughout the game