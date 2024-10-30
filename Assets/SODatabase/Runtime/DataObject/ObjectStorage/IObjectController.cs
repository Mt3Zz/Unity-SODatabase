namespace SODatabase.DataObject
{
    /// <summary>
    /// オブジェクトの操作を制御するためのインターフェースです。
    /// <typeparam name="T">制御する対象のオブジェクトの型。<see cref="BaseObject"/> の派生クラスでなければなりません。</typeparam>
    /// </summary>
    public interface IObjectController<T>
        where T : BaseObject
    {
        /// <summary>
        /// 指定されたオブジェクトを更新します。
        /// </summary>
        /// <param name="obj">更新する対象の <typeparamref name="T"/> インスタンス。</param>
        /// <returns>
        /// オブジェクトが正常に更新された場合は <c>true</c>、それ以外の場合は <c>false</c> を返します。
        /// </returns>
        bool UpdateObject(T obj);
    }
}
