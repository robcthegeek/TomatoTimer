﻿Tomato Timer
============
A small utility to help you succeed with the Pomodoro Technique.. 

Overview
--------

NOTE:
The Pomodoro Technique was not created by the author of Tomato Timer, Francesco Cirillo.
Please view his website at: http://cirillosscrapbook.wordpress.com and the official
Pomodoro Technique homepage at http://www.pomodorotechnique.com.

TomatoTimer aims to be a small but hard-hitting utility to help you succeed
in using the Pomodoro Technique.

Tomato Timer aims to keep the simplicity of the Pomodoro Technique. It's simple
architecture consists of:

 * The "core" - the Timer which is fundamental building block of the entire process.
 * An extensible system that allows you to pick and choose how you work and the core will
   work based on that.

Tomato Timer itself does NOT intend to be a piece of "bloatware" aiming for "one size
fits all". This is simply not how *real* people want to work. Features will be added by
using a plugin system. This allows you to cherry-pick the features you would like, as well
as keep costs down (there may be paid plugins in the future - and why should you pay for 
features you don't want?).


Requirements
------------

.NET Framework 4.0

Release Notes/Updates
---------------------

Version 0.3 (in progress)

Complete re-architecture to accommodate Plugins.
Added first-stage plugins (Tomato Events).
Migrated Mp3Player, NotifyIcon and MiniTimer to Default Plugins.
Improved UI updating in both MiniTimer and Main window.

Version 0.2

Added single-instance app loading support (shows when minimised).
Added basic version display to window title.
Added "Interrupt" to MiniTimer Display.
Sizing improvements to MiniTimer (prevents distraction w/ dynamic sizing).
Sorted output paths in build script (no more "bin\Release" in ZIP).

Version 0.1

File > New Project :)