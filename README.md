# 🌒 Sundown – Working Together on GitHub

Welcome to the GitHub page for **Sundown**, our Unity project.  
🔗 https://github.com/Axi-Alfred/Sundown

This guide is here to help you understand **how we work together using GitHub**.  
Even if you’ve never used Git or GitHub before — no worries! Follow these simple rules and you’ll do great.

---

## 👣 How We Use GitHub (Simple Rules)

### 1. 🔀 Don’t Touch the Main Branch Directly  
Think of `main` as the **finished version** of the project.

**Always create your own branch** when you work on something new.  
A branch is like your own workspace — safe to experiment in.

Examples of branch names:
feature/add-slime-jump fix/maggot-ai level/create-cave3

You can make a new branch in GitHub Desktop or directly in Unity after pulling the latest changes.

---

### 2. 🧠 Only One Person Edits a Scene at a Time  
Unity scenes can break easily if more than one person edits them at the same time.

**Before you start editing a scene**, check with the team:
- Use Discord or our group chat
- Say what scene you're working on
- Wait for the OK if someone else is using it

---

### 3. 🔄 Always Pull Before You Push  
Before you upload your changes to GitHub:

1. Click **Fetch origin** (or "Pull" in GitHub Desktop)
2. Check that your version is up to date
3. Fix any conflicts if needed
4. Then click **Push origin** to upload your changes

This makes sure we don’t accidentally overwrite each other’s work.

---

### 4. ✅ Write Clear Commit Messages  
A commit is like saving your work — with a note.

Good examples:
Add player health system Fix bug with enemy pathfinding Update cave lighting in level 1

Bad examples:
stuff asdf fixed it

Your future self and your teammates will thank you.

---

### 5. 📂 Don’t Upload Unity Junk Files  
There are some files we *never* want in GitHub, like:
- `Library/`
- `Temp/`
- `Build/`
- `.csproj` or `.sln`

These files are computer-specific and break things for others.

We already have a `.gitignore` file that helps block them — **don’t delete or change it.**

---

### 6. 🧪 Test Your Changes Before You Share Them  
When your branch is ready:
- Open it in Unity
- Play the scene
- Make sure nothing’s broken

Then, ask someone to review it (called a **Pull Request** or PR).

Only after everything looks good should it go into the `main` version.

---

### 7. 📢 Communicate Often  
We’re a team. Use Discord or our shared space to:
- Tell people what you're working on
- Ask for help
- Share ideas
- Say when you're done with a branch or scene

This makes everything smoother and friendlier.

---

### 8. ❌ Don’t Use Unity Collaborate or Plastic SCM  
We’re using **GitHub** for version control.  
No need to click anything in Unity about Collaborate or Plastic — ignore those.

---

## 💬 Questions?  
Ask in our Discord or chat group.  
Everyone’s learning — no dumb questions here.

Let’s make Sundown awesome together!
