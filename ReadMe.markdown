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

### Migrate ReadMe.txt to Wiki on GitHub
	
- Create Release Notes Page
- Create Requirements Page
- Create Overview Page
	
### Re-Implement the Async Method Manager

- Design
	- Completely De-Couple the "Method Runner" from the Rest of the Application.
	- Write as Simple Single-Threaded to "Get Running"
	- Spike using the .NET Parallel bits, will they work?
	- What About Plugin *Types* (e.g. Background, Windowed?)
- Re-Build Tests from Ground Up