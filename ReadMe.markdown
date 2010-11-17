# TomatoTimer

> A simple, flexible, extensible WPF app for [Pomodoro](http://www.pomodorotechnique.com/) practitioners.

**WARNING**: Currently, the code is pretty trashed as I am ripping it to pieces (and lost the "clean" build due to system crash and my lack of commitment to a backup procedure :).

## Roadmap

- Re-write the plugin model.
	- Spin Up Default (Internal) Plugins
	- Allow Others to Drop in New Plugins
- Re-write the async model, using .NET parallel core.
- Implement/Review "Basic" Plugin Package
	- Mp3 Player (for Timer Events)
	- Timer Window
	- Simple Task List (Store of Tasks, Due Dates, Estimates, Priority etc.)

## ToDo

- Remove all the "app settings" crap and promote to configuration sections in the Application Config file.
- Remove the user settings - they are either doing Pomodoro or not :P (can re-implement in a cleaner fashion if needed).