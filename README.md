# Milkshake.NET
A library for meme generation, based specifically on [Milkshake Simulator](https://github.com/MateusF03/MilkshakeSimulator/tree/master), though also [Feijoada Simulator](https://feijoadasimulator.top/) and [ShitpostBot 5000](https://www.shitpostbot.com/), using ImageMagick ([Magick.NET](https://github.com/dlemstra/Magick.NET/)).
It was developed for its use on Discord, but it possibly runs on any platform.

## Requirements
Milkshake.NET requires .NET 7 and [Magick.NET](https://github.com/dlemstra/Magick.NET/) for functioning.

## Documentation
Proper documentation and guide will be written after I'm done with this semester on college.
The core of the library revolves around the *Milkshakes*, which are the main objects, they are:
1. **The Source** - Any image added with the intent of serving as the "protagonist" of a generated image. A source can have tags to specify its type.
2. **The Template** - The image used as a base for the Generation. Sources will be placed either over or under the Template.
3. **The Topping** - A set of properties inherently dependant of the designated Template. A topping can also have tags.
4. **The Generation** - It is the resulting *Milkshake*, being the metadata of the generated image.

### Tags
There are 8 tags the user can choose:
1. Any
2. Person
3. Symbol
4. Object
5. Shitpost
6. Picture
7. Post
8. Text

### Milkshake Instance
An Instance is the context for the data Milkshake.NET can access, there can be multiple Instances.

## Credits
This project is totally inspired on my friend Mateusão's [Milkshake Simulator](https://github.com/MateusF03/MilkshakeSimulator/tree/master) project, which is a Discord bot with the same base functionality.