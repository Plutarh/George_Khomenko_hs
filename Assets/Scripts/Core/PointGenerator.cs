using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter _ground;
    [SerializeField] private List<Vector3> _corners = new List<Vector3>();

    [SerializeField] private List<Point> _points = new List<Point>();
    [SerializeField] private float _minDistanceBetweenPoints = 2;
    [SerializeField] private int _maxPatrollPointsCount = 20;

    [SerializeField] private Point _patrollPointPrefab;
    [SerializeField] private Point _basePointPrefab;


    private Bounds _groundBounds;


    public void Initialize()
    {
        ReadConfig();
        FindBoundsOnGround();
        FindCornersOnGround();
        CreateBasePoint();
        CreatePatrollPoints();
    }

    void ReadConfig()
    {
        _minDistanceBetweenPoints = Config.Instance.minDistanceBetweenPoints;
        _maxPatrollPointsCount = Config.Instance.maxPatrollPointsCount;
    }

    void CreateBasePoint()
    {
        var randomPoint = GetRandomPointOnGround();

        Point point = Instantiate(_basePointPrefab, transform);

        point.pointType = EPointType.Base;
        point.transform.localPosition = randomPoint;

        _points.Add(point);

        PointsHolder.allPoints.Add(point);
    }

    void CreatePatrollPoints()
    {
        // Кол-во попыток поиска точки
        int tryFindRandomAvaliablePointCount = 300;

        while (_points.Count < _maxPatrollPointsCount && tryFindRandomAvaliablePointCount > 0)
        {
            var randomPoint = GetRandomPointOnGround();

            if (_points.Any(p => Vector3.Distance(p.transform.localPosition, randomPoint) < _minDistanceBetweenPoints))
            {
                tryFindRandomAvaliablePointCount--;
            }
            else
            {
                tryFindRandomAvaliablePointCount = 300;

                Point point = Instantiate(_patrollPointPrefab, transform);

                point.pointType = EPointType.Patroll;
                point.transform.localPosition = randomPoint;

                _points.Add(point);
                PointsHolder.allPoints.Add(point);
            }
        }
    }


    Vector3 GetRandomPointOnGround()
    {
        var point = new Vector3(Random.Range(_groundBounds.min.x, _groundBounds.max.x), _ground.transform.position.y, Random.Range(_groundBounds.min.z, _groundBounds.max.z));
        point = _groundBounds.ClosestPoint(point);
        return _ground.transform.TransformPoint(point);
    }

    void FindBoundsOnGround()
    {
        if (_ground == null)
        {
            Debug.LogError("No ground for find bounds");
            return;
        }

        _groundBounds = _ground.sharedMesh.bounds;
    }

    // TODO возможно удалить корнеры
    void FindCornersOnGround()
    {
        if (_groundBounds == null)
        {
            Debug.LogError("No bounds for ground");
            return;
        }

        Vector3 leftBottomCorner = _ground.transform.TransformPoint(new Vector3(_groundBounds.min.x, _ground.transform.position.y, _groundBounds.min.z));
        _corners.Add(leftBottomCorner);

        Vector3 leftTopCorner = _ground.transform.TransformPoint(new Vector3(_groundBounds.min.x, _ground.transform.position.y, _groundBounds.max.z));
        _corners.Add(leftTopCorner);

        Vector3 rightTopCorner = _ground.transform.TransformPoint(new Vector3(_groundBounds.max.x, _ground.transform.position.y, _groundBounds.max.z));
        _corners.Add(rightTopCorner);

        Vector3 rightBottomCorner = _ground.transform.TransformPoint(new Vector3(_groundBounds.max.x, _ground.transform.position.y, _groundBounds.min.z));
        _corners.Add(rightBottomCorner);
    }



    private void OnDrawGizmos()
    {
        if (_groundBounds == null) return;

        Gizmos.color = Color.red;

        _corners.ForEach(c => Gizmos.DrawSphere(c, 0.2f));
    }
}

public enum EPointType
{
    Base,
    Patroll
}

