public class ForestHutWH : Warehouse
{
	public override ResHolder ResNeedToMove()
	{
		ResHolder resHolder = this.GetResource(ResourceName.logwood);
		if (resHolder.Current() > 0) return resHolder;
		return null;
	}
}
