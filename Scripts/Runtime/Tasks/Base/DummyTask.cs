/*  Copyright (C) 2022 Unit for Visually Impaired People (UVIP) - Fondazione Istituto Italiano di Tecnologia (IIT)
    Author: Davide Esposito
    email: davide.esposito@iit.it | spsdvd48@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;


namespace SALLO
{
    /// <summary>
    /// Dummy Task class. Needed for unit testing. Please ignore
    /// </summary>
    public class DummyTask : Task
    {
        public override void Run(float? timeOn = null, float? timeOff = null)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetAnswer<T>(T answer = default)
        {
            throw new System.NotImplementedException();
        }

        protected override void PositionCheckSetUp()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator ShowStimulus(CylindricalCoordinates stimulus, float timeOn)
        {
            throw new System.NotImplementedException();
        }
    }
}
