namespace Game.Scripts.Services
{
	public class GameReferenceAccessService<T>
	{
		private T reference;
		public T Reference => reference;

		public void SetReference(T reference)
		{
			this.reference = reference;
		}
	}
}
