using UnityEngine;

public class PressableAbility : Pressable
{
	[SerializeField] protected BuildType buildType;
	public override void Pressed()
    {
		string buildName = buildType.ToString();

		Debug.Log("PressableAbility: " + buildName);
		BuildManager.instance.CurrentBuildSet(buildName);
	}

	public virtual string GetBuildType()
	{
		return this.buildType.ToString();
	}
}
